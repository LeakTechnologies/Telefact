# Changelog

All notable changes to the Telefact project will be documented in this file.

## [0.3.0] - 2025-04-16
### Added
- Project initialization using Windows Forms.
- Basic TeletextRenderer implementation that delegates rendering to header and footer components.
- **TeletextHeader**:
  - Renders fixed header elements including:
    - Left page number (" P100")
    - Service name ("Telefact") with a red background and yellow text.
    - Right page number ("100")
    - Timestamp (formatted as "MMM dd HH:mm:ss") right aligned.
- **TeletextFooter**:
  - Renders a footer line with dynamic row information.
  - Implements a grid-based layout with a white background for content cells and black padding.
  
*Initial commit representing a basic working version of Telefact with core UI rendering.*
