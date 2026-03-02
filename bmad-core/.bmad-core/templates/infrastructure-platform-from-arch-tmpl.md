# {{Project Name}} Platform Infrastructure Implementation

[[LLM: Initial Setup

1. Replace {{Project Name}} with the actual project name throughout the document
2. Gather and review required inputs:

   - **Infrastructure Architecture Document** (Primary input - REQUIRED)
   - Liberty infrastructure patterns from environment/ and app-infra/ directories
   - constants.mk and variables.mk configuration files
   - Azure DevOps pipeline configurations
   - Infrastructure Checklist
   - NOTE: If Infrastructure Architecture Document is missing, HALT and request: "I need the Infrastructure Architecture Document to proceed with platform implementation. This document defines the infrastructure design that we'll be implementing."

3. Validate that the infrastructure architecture has been reviewed and approved
4. <critical_rule>All platform implementation must align with Liberty's standardized patterns. Use Make workflows exclusively - no direct terraform commands.</critical_rule>
5. <critical_rule>Follow the environment/ and app-infra/ separation pattern strictly.</critical_rule>

Output file location: `docs/platform-infrastructure/platform-implementation.md`]]

## Executive Summary

[[LLM: Provide a high-level overview of the platform infrastructure being implemented, referencing the infrastructure architecture document's key decisions and Liberty's standardized patterns.]]

- Platform implementation scope and objectives
- Key architectural decisions being implemented
- Liberty pattern compliance and standards
- Expected outcomes and benefits
- Timeline and milestones

## Liberty Infrastructure Standards Alignment

[[LLM: Document how this implementation follows Liberty's standardized patterns and practices.]]

### Pattern Compliance

- Environment/App-infra separation confirmed
- Make workflow implementation verified
- Multi-environment strategy aligned (localdev, sandbox-shared, production)
- Team configuration from constants.mk applied

### Azure Standards

- Subscription model following team allocations
- Resource naming conventions per Liberty standards
- Tagging strategy for cost allocation
- RBAC and security patterns

## Foundation Infrastructure Layer

[[LLM: Implement the base infrastructure layer using Liberty's environment/ pattern. This forms the foundation for all platform services.]]

### Environment Setup

```bash
# Navigate to environment directory
cd environment

# Initialize and plan infrastructure
make init plan

# Review the plan output carefully
# Only proceed if plan shows expected changes

# Apply infrastructure (after approval)
make apply
```

### Resource Group Structure

```hcl
# Example from environment-resource-group.tf
resource "azurerm_resource_group" "instance" {
  for_each = local.instances
  
  name     = each.value.name
  location = each.value.location
  tags     = local.common_tags
}
```

### Workload Identity Configuration

```hcl
# Example workload identity setup
resource "azurerm_user_assigned_identity" "workload_identity" {
  name                = "${var.APPLICATION_NAME_SHORT}-workload-identity-${var.ENVIRONMENT_NAME_SHORT}"
  resource_group_name = local.instances.one.name
  location            = local.instances.one.location
  tags                = local.common_tags
}
```

### Network Foundation

```hcl
# VNET configuration following Liberty patterns
module "network" {
  source = "git@ssh.dev.azure.com:v3/liberty-financial/Templates/az-network-module"
  
  vnet_name           = "${var.APPLICATION_NAME}-vnet-${var.ENVIRONMENT_NAME}"
  address_space       = ["10.0.0.0/16"]
  resource_group_name = azurerm_resource_group.instance["one"].name
  location            = var.LOCATION_INSTANCE_ONE
  
  subnets = {
    aks = {
      address_prefixes = ["10.0.1.0/24"]
      service_endpoints = ["Microsoft.Storage", "Microsoft.Sql"]
    }
    private_endpoints = {
      address_prefixes = ["10.0.2.0/24"]
    }
  }
  
  tags = local.common_tags
}
```

### Security Foundation

- IAM roles and policies
- Security groups and NACLs
- Encryption keys (KMS/Key Vault)
- Compliance controls

### Core Services

- DNS configuration
- Certificate management
- Logging infrastructure
- Monitoring foundation

[[LLM: Platform Layer Elicitation
After implementing foundation infrastructure, present:
"For the Foundation Infrastructure layer, I can explore:

1. **Platform Layer Security Hardening** - Additional security controls and compliance validation
2. **Performance Optimization** - Network and resource optimization
3. **Operational Excellence Enhancement** - Automation and monitoring improvements
4. **Platform Integration Validation** - Verify foundation supports upper layers
5. **Developer Experience Analysis** - Foundation impact on developer workflows
6. **Disaster Recovery Testing** - Foundation resilience validation
7. **BMAD Workflow Integration** - Cross-agent support verification
8. **Finalize and Proceed to Application Infrastructure**

Select an option (1-8):"]]

## Application Infrastructure Implementation

[[LLM: Implement application-specific infrastructure using Liberty's app-infra/ pattern. This builds on top of the foundation layer.]]

### Application Infrastructure Setup

```bash
# Navigate to app-infra directory
cd app-infra

# Initialize and plan infrastructure
make init plan

# Review the plan output carefully
# Verify it references resources from environment layer

# Apply infrastructure (after approval)
make apply
```

### Storage Account Configuration

```hcl
# Example from storage.tf
resource "azurerm_storage_account" "storage" {
  for_each = var.storage_accounts

  name                     = each.value.name
  resource_group_name      = data.azurerm_resource_group.instance_one.name
  location                 = data.azurerm_resource_group.instance_one.location
  account_tier             = each.value.account_tier
  account_replication_type = each.value.replication_type
  
  network_rules {
    default_action = "Deny"
    ip_rules       = var.allowed_ips
    virtual_network_subnet_ids = [
      data.azurerm_subnet.aks.id
    ]
  }
  
  identity {
    type = "SystemAssigned"
  }
  
  tags = local.common_tags
}
```

### Cognitive Services Setup

```hcl
# Example from cognitive-account.tf
resource "azurerm_cognitive_account" "openai" {
  name                = "${var.APPLICATION_NAME_SHORT}-openai-${var.ENVIRONMENT_NAME_SHORT}"
  location            = data.azurerm_resource_group.instance_one.location
  resource_group_name = data.azurerm_resource_group.instance_one.name
  kind                = "OpenAI"
  sku_name           = "S0"
  
  custom_subdomain_name = "${var.APPLICATION_NAME}-${var.ENVIRONMENT_NAME}"
  
  network_acls {
    default_action = "Deny"
    ip_rules       = var.allowed_ips
  }
  
  tags = local.common_tags
}

# OpenAI Model Deployment
resource "azurerm_cognitive_deployment" "gpt4" {
  name                 = "gpt-4o"
  cognitive_account_id = azurerm_cognitive_account.openai.id
  
  model {
    format  = "OpenAI"
    name    = "gpt-4o"
    version = "2024-11-20"
  }
  
  scale {
    type = "Standard"
  }
}
```

### Environment Variable Configuration

```bash
# After app-infra deployment, update config.sh
source ./config.sh

# Verify outputs
echo $AZURE_OPEN_AI_ENDPOINT
echo $ALCHEMIST_STORAGE_ACCOUNT_NAME
```

## Container Platform Implementation

[[LLM: Build the container orchestration platform on top of the foundation infrastructure, following the architecture's container strategy.]]

### Kubernetes Cluster Setup

^^CONDITION: uses_eks^^

```bash
# EKS Cluster Configuration
eksctl create cluster \
  --name {{cluster_name}} \
  --region {{aws_region}} \
  --nodegroup-name {{nodegroup_name}} \
  --node-type {{instance_type}} \
  --nodes {{node_count}}
```

^^/CONDITION: uses_eks^^

^^CONDITION: uses_aks^^

```bash
# AKS Cluster Configuration
az aks create \
  --resource-group {{resource_group}} \
  --name {{cluster_name}} \
  --node-count {{node_count}} \
  --node-vm-size {{vm_size}} \
  --network-plugin azure
```

^^/CONDITION: uses_aks^^

### Node Configuration

- Node groups/pools setup
- Autoscaling configuration
- Node security hardening
- Resource quotas and limits

### Cluster Services

- CoreDNS configuration
- Ingress controller setup
- Certificate management
- Storage classes

### Security & RBAC

- RBAC policies
- Pod security policies/standards
- Network policies
- Secrets management

[[LLM: Present container platform elicitation options similar to foundation layer]]

## GitOps Workflow Implementation

[[LLM: Implement GitOps patterns for declarative infrastructure and application management as defined in the architecture.]]

### GitOps Tooling Setup

^^CONDITION: uses_argocd^^

```yaml
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: argocd
  namespace: argocd
spec:
  source:
    repoURL:
      "[object Object]": null
    targetRevision:
      "[object Object]": null
    path:
      "[object Object]": null
```

^^/CONDITION: uses_argocd^^

^^CONDITION: uses_flux^^

```yaml
apiVersion: source.toolkit.fluxcd.io/v1beta2
kind: GitRepository
metadata:
  name: flux-system
  namespace: flux-system
spec:
  interval: 1m
  ref:
    branch:
      "[object Object]": null
  url:
    "[object Object]": null
```

^^/CONDITION: uses_flux^^

### Repository Structure

```text
platform-gitops/
   clusters/
      production/
      staging/
      development/
   infrastructure/
      base/
      overlays/
```

### Deployment Workflows

- Application deployment patterns
- Progressive delivery setup
- Rollback procedures
- Multi-environment promotion

### Access Control

- Git repository permissions
- GitOps tool RBAC
- Secret management integration
- Audit logging

## Service Mesh Implementation

[[LLM: Deploy service mesh for advanced traffic management, security, and observability as specified in the architecture.]]

^^CONDITION: uses_istio^^

### Istio Service Mesh

```bash
# Istio Installation
istioctl install --set profile={{istio_profile}} \
  --set values.gateways.istio-ingressgateway.type={{ingress_type}}
```

- Control plane configuration
- Data plane injection
- Gateway configuration
- Observability integration
  ^^/CONDITION: uses_istio^^

^^CONDITION: uses_linkerd^^

### Linkerd Service Mesh

```bash
# Linkerd Installation
linkerd install --cluster-name={{cluster_name}} | kubectl apply -f -
linkerd viz install | kubectl apply -f -
```

- Control plane setup
- Proxy injection
- Traffic policies
- Metrics collection
  ^^/CONDITION: uses_linkerd^^

### Traffic Management

- Load balancing policies
- Circuit breakers
- Retry policies
- Canary deployments

### Security Policies

- mTLS configuration
- Authorization policies
- Rate limiting
- Network segmentation

## Developer Experience Platform

[[LLM: Build the developer self-service platform to enable efficient development workflows as outlined in the architecture.]]

### Developer Portal

- Service catalog setup
- API documentation
- Self-service workflows
- Resource provisioning

### CI/CD Integration

```yaml
apiVersion: tekton.dev/v1beta1
kind: Pipeline
metadata:
  name: platform-pipeline
spec:
  tasks:
    - name: build
      taskRef:
        name: build-task
    - name: test
      taskRef:
        name: test-task
    - name: deploy
      taskRef:
        name: gitops-deploy
```

### Development Tools

- Local development setup
- Remote development environments
- Testing frameworks
- Debugging tools

### Self-Service Capabilities

- Environment provisioning
- Database creation
- Feature flag management
- Configuration management

## Platform Integration & Security Hardening

[[LLM: Implement comprehensive platform-wide integration and security controls across all layers.]]

### End-to-End Security

- Platform-wide security policies
- Cross-layer authentication
- Encryption in transit and at rest
- Compliance validation

### Integrated Monitoring

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: prometheus-config
data:
  prometheus.yml: |
    global:
      scrape_interval: {{scrape_interval}}
    scrape_configs:
      - job_name: 'kubernetes-pods'
        kubernetes_sd_configs:
          - role: pod
```

### Platform Observability

- Metrics aggregation
- Log collection and analysis
- Distributed tracing
- Dashboard creation

### Backup & Disaster Recovery

- Platform backup strategy
- Disaster recovery procedures
- RTO/RPO validation
- Recovery testing

## Platform Operations & Automation

[[LLM: Establish operational procedures and automation for platform management.]]

### Monitoring & Alerting

- SLA/SLO monitoring
- Alert routing
- Incident response
- Performance baselines

### Automation Framework

```yaml
apiVersion: operators.coreos.com/v1alpha1
kind: ClusterServiceVersion
metadata:
  name: platform-operator
spec:
  customresourcedefinitions:
    owned:
      - name: platformconfigs.platform.io
        version: v1alpha1
```

### Maintenance Procedures

- Upgrade procedures
- Patch management
- Certificate rotation
- Capacity management

### Operational Runbooks

- Common operational tasks
- Troubleshooting guides
- Emergency procedures
- Recovery playbooks

## BMAD Workflow Integration

[[LLM: Validate that the platform supports all BMAD agent workflows and cross-functional requirements.]]

### Development Agent Support

- Frontend development workflows
- Backend development workflows
- Full-stack integration
- Local development experience

### Infrastructure-as-Code Development

- IaC development workflows
- Testing frameworks
- Deployment automation
- Version control integration

### Cross-Agent Collaboration

- Shared services access
- Communication patterns
- Data sharing mechanisms
- Security boundaries

### CI/CD Integration

```yaml
stages:
  - analyze
  - plan
  - architect
  - develop
  - test
  - deploy
```

## Platform Validation & Testing

[[LLM: Execute comprehensive validation to ensure the platform meets all requirements.]]

### Functional Testing

- Component testing
- Integration testing
- End-to-end testing
- Performance testing

### Security Validation

- Penetration testing
- Compliance scanning
- Vulnerability assessment
- Access control validation

### Disaster Recovery Testing

- Backup restoration
- Failover procedures
- Recovery time validation
- Data integrity checks

### Load Testing

```typescript
// K6 Load Test Example
import http from 'k6/http';
import { check } from 'k6';

export let options = {
  stages: [
    { duration: '5m', target: {{target_users}} },
    { duration: '10m', target: {{target_users}} },
    { duration: '5m', target: 0 },
  ],
};
```

## Knowledge Transfer & Documentation

[[LLM: Prepare comprehensive documentation and knowledge transfer materials.]]

### Platform Documentation

- Architecture documentation
- Operational procedures
- Configuration reference
- API documentation

### Training Materials

- Developer guides
- Operations training
- Security best practices
- Troubleshooting guides

### Handoff Procedures

- Team responsibilities
- Escalation procedures
- Support model
- Knowledge base

## Implementation Review with Architect

[[LLM: Document the post-implementation review session with the Architect to validate alignment and capture learnings.]]

### Implementation Validation

- Architecture alignment verification
- Deviation documentation
- Performance validation
- Security review

### Lessons Learned

- What went well
- Challenges encountered
- Process improvements
- Technical insights

### Future Evolution

- Enhancement opportunities
- Technical debt items
- Upgrade planning
- Capacity planning

### Sign-off & Acceptance

- Architect approval
- Stakeholder acceptance
- Go-live authorization
- Support transition

## Platform Metrics & KPIs

[[LLM: Define and implement key performance indicators for platform success measurement.]]

### Technical Metrics

- Platform availability: {{availability_target}}
- Response time: {{response_time_target}}
- Resource utilization: {{utilization_target}}
- Error rates: {{error_rate_target}}

### Business Metrics

- Developer productivity
- Deployment frequency
- Lead time for changes
- Mean time to recovery

### Operational Metrics

- Incident response time
- Patch compliance
- Cost per workload
- Resource efficiency

## Appendices

### A. Configuration Reference

[[LLM: Document all configuration parameters and their values used in the platform implementation.]]

### B. Troubleshooting Guide

[[LLM: Provide common issues and their resolutions for platform operations.]]

### C. Security Controls Matrix

[[LLM: Map implemented security controls to compliance requirements.]]

### D. Integration Points

[[LLM: Document all integration points with external systems and services.]]

[[LLM: Final Review - Ensure all platform layers are properly implemented, integrated, and documented. Verify that the implementation fully supports the BMAD methodology and all agent workflows. Confirm successful validation against the infrastructure checklist.]]

---

_Platform Version: 1.0_
_Implementation Date: {{implementation_date}}_
_Next Review: {{review_date}}_
_Approved by: {{architect_name}} (Architect), {{devops_name}} (DevOps/Platform Engineer)_
