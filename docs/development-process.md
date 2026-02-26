# Development Process

## Approach

AI-assisted, TDD-driven development using the BMAD Core agent framework inside Cursor IDE. Three specialized / customized agents — **Architect**, **Product Owner**, and **Developer** — collaborated with a human developer to go from requirements to working software.

## Tools

- **C# 12 / .NET 8.0 LTS** — records, primary constructors, switch expressions, pattern matching
- **xUnit + FluentAssertions** — unit testing with readable assertions
- **Cursor IDE** — AI pair programming with BMAD agents (Opus 4.6)
- **Git** — version control with TDD-reflective commits

## Resources Consulted

- Toy Robot Challenge specification (PDF)
- [BMAD Method Documentation](https://docs.bmad-method.org/) — framework behind the AI agent workflow

## Key Prompts

| Prompt | Impact |
|--------|--------|
| *"Activate architect agent — architect a solution for the Toy Robot challenge"* | Defined architecture, patterns, and tech stack with human as a Director |
| *"Ask me any clarifying questions before making assumptions"* | Ensured agents gathered requirements before generating outputs |
| *"Make this document super optimized for AI agents"* | Rewrote architecture.md for zero-ambiguity code generation |
| *"Activate PO agent — create user stories based on architecture document"* | Generated 5 TDD-structured stories with exact code snippets |
| *"Activate dev agent"* + *"Stop when I have to commit to show TDD approach"* | Implementation with TDD and enforced commits |

## Key Decisions & Trade-offs

| Decision | Why |
|----------|-----|
| Command Pattern over switch statement | Open for extension — adding `SHOW` on top of `REPORT` required zero changes to existing classes |
| `record` for Position, `class` for Robot | Value semantics vs mutable state — type system enforces the distinction |
| No DI container | Overkill for this size; `new` in `Program.cs` is simpler and fully testable |
| `[Collection("ConsoleOutput")]` on tests | xUnit runs classes in parallel; tests capturing `Console.Out` need sequential execution |
| Silent ignore for invalid commands | Spec-driven — `CommandParser` returns `null`, `Simulator` skips it |
| Dynamic table size (bonus) | Spec says 5×5 but we made it configurable via CLI args at no extra complexity |
| Manual validation for Program.cs | Thin glue code (16 lines) — piped test data files beat complex console mocking |
