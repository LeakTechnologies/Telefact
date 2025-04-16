# Changelog

All notable changes to the Telefact project will be documented in this file.

## [0.3.8] - 2025-04-16
### Changed
- Updated TeletextContent rendering to enforce strict grid alignment for the story content by drawing each character individually.
  - The header is already drawn character-by-character and centered in yellow.
  - The story content is now also rendered character-by-character (starting from the configured left padding) to ensure it remains aligned with the grid.
- This fix ensures that all text (both header and content) adheres strictly to the Teletext/Modeseven grid layout.
  
## [0.3.7] - 2025-04-16
### Changed
- Updated TeletextContent to reserve an extra blank row at the bottom for footer breathing room and maintained consistent left/right padding.
- (Next, the header was centered with yellow text, and now content alignment is enforced.)

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
