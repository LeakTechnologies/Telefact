# Changelog

All notable changes to the Telefact project will be documented in this file.

## [0.3.4] - 2025-04-16
### Changed
- **Introduced** a fixed teletext grid layout (40 columns × 25 rows) via a new `TeletextGrid` class.
- **Updated** the `Renderer` to calculate `cellWidth` and `cellHeight` based on the client window size and pass them to `TeletextFooter`.
- **Modified** the `TeletextFooter` to accept dynamic cell sizes, making the footer position align with the bottom row of the grid.
- **Ensured** consistency with a more authentic Teletext/Modeseven terminal look across different window sizes.

## [0.3.3] - 2025-04-16
### Changed
- Updated the timestamp rendering in the header by adding a timer in **MainForm.cs** to refresh the UI every second, ensuring the displayed timestamp updates in real time.

## [0.3.2] - 2025-04-16
### Fixed
- Removed extraneous top-level code from **Program.cs** to resolve build issues.

## [0.3.1] - 2025-04-16
### Moved
- Moved documentation files (**README.md**, **CHANGELOG.md**, and **ROADMAP.md**) into the **Markdown** folder.

## [0.3.0] - 2025-04-16
### Added
- Initial commit featuring core UI rendering with Windows Forms.
- Implemented **TeletextHeader** to display fixed header elements, including:
  - Left page number (" P100")
  - Service name ("Telefact") with a red background and yellow text
  - Right page number ("100")
  - Timestamp (formatted as "MMM dd HH:mm:ss") right aligned.
- Implemented **TeletextFooter** to display dynamic footer information with a grid-based layout.
