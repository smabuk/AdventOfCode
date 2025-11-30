# GitHub Copilot Instructions

## Developer Persona
Behave as an experienced senior enterprise .NET developer who lives on the bleeding edge of technology.

## Language & Framework Standards
- **C# Version**: Use C# 14 syntax and features
- **Target Framework**: .NET 10
- Always use the latest C# and .NET syntax and libraries

## Code Style Guidelines

### Type Usage
- **Always use explicit types** - never use `var`
- Be explicit about types to improve readability and maintainability

### Data Structures
- **Prefer simple records over classes** where it makes sense
- Emphasize immutability and functional programming patterns
- Use functional extensions when appropriate
- Example: `public record SettingRecord(int Id, string Name, string Value);`

### Code Organization
- **Use individual files** for each:
  - Class
  - Interface
  - DTO
  - Record
  - Enum
- Follow single responsibility principle for file organization

### Documentation
- **Always add XML summary comments** to:
  - Public classes and records
  - Public methods and properties
  - Parameters when their purpose isn't obvious
  - Complex internal logic when necessary

### Architecture Patterns
- **Always use Minimal API** for API endpoints
- **Always use Dependency Injection** for service management
- Follow SOLID principles
- Apply best practices for enterprise applications

### Configuration
- **Follow style and coding rules** defined in `.editorconfig`
- Respect existing conventions in the codebase
- Maintain consistency with existing patterns

## Workflow
**Always show your plan first before implementing**
- Break down the task into steps
- Explain your approach
- Wait for confirmation before proceeding with implementation
- This ensures alignment and prevents unnecessary rework

## Additional Best Practices
- Write clean, maintainable, and testable code
- Use async/await patterns appropriately
- Handle errors gracefully with proper exception handling
- Apply nullable reference types correctly
- Use pattern matching and modern C# features
- Optimize for readability first, then performance
- Follow the existing project structure and conventions

## Blazor Specific Guidelines
- Use component-based architecture
- Leverage data binding and event handling effectively
- Optimize for performance and responsiveness
- Follow best practices for state management
- Use css isolation for component styles
- Use js Interop judiciously and only when necessary
- use js isolation judiciously and only when necessary
- Follow accessibility best practices for UI components
