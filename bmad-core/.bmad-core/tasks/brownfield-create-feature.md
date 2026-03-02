# Create Brownfield Feature Task

## Purpose

Create a single feature within a brownfield epic that groups related user stories delivering specific functionality. This task bridges between epic-level planning and story-level implementation for brownfield enhancements.

## When to Use This Task

**Use this task when:**

- You have an approved brownfield epic requiring feature breakdown
- The functionality can be grouped into 2-5 related user stories
- The feature delivers a distinct capability within the epic
- Integration follows established patterns within feature scope

**Skip features and use stories directly when:**

- The epic only needs 1-3 stories total
- Stories don't naturally group into features
- Adding features creates unnecessary 1:1 mappings

## Instructions

### 1. Feature Analysis

Before creating the feature, review the parent epic and identify:

**Epic Context:**

- [ ] Parent epic goals and scope understood
- [ ] Feature's role within the epic identified
- [ ] Related functionality grouping is logical
- [ ] Integration patterns from epic noted

**Feature Scope:**

- [ ] Distinct functionality clearly defined
- [ ] Value proposition established
- [ ] Story grouping makes sense (2-5 stories)
- [ ] Dependencies within epic identified

### 2. Feature Creation

Create a focused feature following this structure:

#### Feature Title

Feature {{EpicNum}}.{{FeatureNum}}: {{Feature Name}}

#### Benefit Hypothesis

{{1-2 sentences describing the value this feature brings to users and why it's worth building}}

#### Business Value

{{Specific measurable value to the business, including integration benefits}}

#### Feature Context

**Integration with Existing System:**

- Enhances: {{existing functionality being enhanced}}
- Integration points: {{specific integration touchpoints}}
- Follows patterns: {{existing patterns to follow}}
- Dependencies: {{other features or existing components}}

#### Acceptance Criteria (Feature Level)

- [ ] Core functionality works end-to-end with existing system
- [ ] Integration points are stable and tested
- [ ] Performance meets requirements in integrated environment
- [ ] Existing functionality remains unaffected
- [ ] Feature delivers promised value

#### User Stories

List 2-5 focused stories that complete this feature:

1. **Story {{FeatureNum}}.1:** {{Story title focusing on core functionality}}
2. **Story {{FeatureNum}}.2:** {{Story title for integration aspects}}
3. **Story {{FeatureNum}}.3:** {{Story title for additional capability}}

#### Technical Considerations

- **Integration Pattern:** {{how this feature integrates}}
- **Constraints:** {{existing system constraints}}
- **Performance:** {{considerations for integrated system}}

### 3. Validation Checklist

Before finalizing the feature:

**Scope Validation:**

- [ ] Feature represents distinct functionality
- [ ] Stories naturally group under this feature
- [ ] Not creating feature for feature's sake
- [ ] Aligns with parent epic goals

**Integration Check:**

- [ ] Integration approach is clear
- [ ] Dependencies are identified
- [ ] Risk to existing system assessed
- [ ] Rollback approach considered

### 4. Handoff to Story Creation

Once feature is defined:

---

**Story Creation Guidance:**

"Create detailed user stories for Feature {{FeatureNum}}: {{Feature Name}}

Key considerations:

- Parent feature goal: {{benefit hypothesis}}
- Integration pattern: {{integration approach}}
- Each story should deliver vertical slice of functionality
- Maintain existing system integrity throughout

Stories should sequence logically to build the feature incrementally."

---

## Success Criteria

Feature creation is successful when:

1. Feature delivers distinct value within the epic
2. Stories logically group under the feature
3. Integration approach respects existing architecture
4. Feature can be delivered incrementally
5. Clear value proposition justifies the feature

## Important Notes

- Features are optional - only use when they add clarity
- Don't force features if stories naturally flow from epic
- Each feature should be releasable when all stories complete
- Consider feature flags for safe rollout in brownfield contexts 