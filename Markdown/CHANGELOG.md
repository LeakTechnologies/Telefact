<!-- CHANGELOG.md -->
# Changelog

All notable changes to the Telefact project are documented in this file.

## [0.4.3] – 2025-04-23
### Changed
- **TeletextRSSContent** now enforces strict per-character, grid-aligned rendering on both index and story pages.
- Index pages:
  - Reduced to 5 headlines.
  - Added blank “breathing” row between entries.
  - Dot-leaders in cyan linking headlines to their page numbers.
- Story pages:
  - Title rendered in **white**, uppercase.
  - Body rendered in **cyan**, preserving original case.
  - Improved HTML-stripping and description fallback logic.
  - Wrapped text strictly to `PageWidth` columns.
- Minor refactor of `Renderer` and `MainForm` to support new RSS pagination logic.

## [0.4.2] – 2025-04-16
### Changed
- Moved debug “Story of Teletext” page from 100 to 777. Static story now appears only on page 777 when `DebugStaticStoryEnabled` is `true`.

## [0.4.1] – 2025-04-16
### Added
- `ConfigManager.DebugStaticStoryEnabled` toggle (via App.config) to switch header to “Story of Teletext” for debug mode.
- `TeletextContent` reads that flag and displays the appropriate 3-line header.

## [0.4.0] – 2025-04-16
### Added
- `RssCacheManager` for 15-minute caching of CBC RSS feeds.
- `TeletextRSSContent` to render RSS-based Teletext pages (300–309, 310–319, etc.) with index and story pages.
- Extended `Renderer` with a `PageNumber` property and branching logic to delegate to `TeletextRSSContent` or `TeletextContent`.

## [0.3.8] – 2025-04-16
### Changed
- Enforced strict grid alignment for all story content by drawing each character individually.
- Header already drawn character-by-character and centered in yellow.
  
## [0.3.7] – 2025-04-16
### Changed
- Reserved an extra blank row at the bottom for footer breathing room.
- Maintained consistent left/right padding across content.

## [0.3.5] – 2025-04-16
### Added
- Automatic subpage rotation feature in the `TeletextContent` component.
  - Subpages cycle automatically every 10 seconds.
  - `MainForm`’s timer updated to trigger subpage rotation.

## [0.3.4] – 2025-04-16
### Changed
- Introduced a fixed teletext grid layout (40 columns × 25 rows) via a new `TeletextGrid` class.
- Updated the `Renderer` to calculate `cellWidth` and `cellHeight` dynamically.
- Modified `TeletextFooter` to accept dynamic cell sizes for proper bottom-row alignment.

## [0.3.3] – 2025-04-16
### Changed
- Updated header timestamp to refresh in real time via a 1 Hz `Timer` in `MainForm.cs`.

## [0.3.2] – 2025-04-16
### Fixed
- Miscellaneous bug fixes and code cleanup.

