# Changelog

All notable changes to the Telefact project will be documented in this file.

## [0.3.5] - 2025-04-16
### Added
- Automatic subpage rotation feature in the TeletextContent component.
  - Subpages now cycle automatically every 10 seconds.
  - MainForm’s timer has been updated to trigger subpage rotation.
  
## [0.3.4] - 2025-04-16
### Changed
- Introduced a fixed teletext grid layout (40 columns × 25 rows) via a new TeletextGrid class.
- Updated the Renderer to calculate cellWidth and cellHeight based on the client window size and pass them to TeletextFooter.
- Modified TeletextFooter to accept dynamic cell sizes, making the footer align with the bottom row of the grid.
- Ensured consistency with a more authentic Teletext/Modeseven terminal look across different window sizes.

## [0.3.3] - 2025-04-16
### Changed
- Updated the timestamp rendering in the header by adding a timer in MainForm.cs to refresh the UI every second, ensuring the displayed timestamp updates in real time.

## [0.3.2] - 2025-04-16
### Fixed
