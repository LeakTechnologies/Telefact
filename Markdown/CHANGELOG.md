# Changelog

All notable changes to the Telefact project will be documented in this file.

## [0.4.5] - 2025-04-23
### Changed
- TeletextRSSContent now reserves **3 header rows + 1 blank row** before any page content, so everything aligns properly under the header.

## [0.4.4] - 2025-04-23
### Changed
- Reduced `StoriesPerCategory` to **5**; index pages now list 5 headlines, and story pages cover exactly pages X1–X5.
- Headlines rendered in **WHITE uppercase**, body text in **CYAN** original case.
- Introduced **dot-leader** between the end of each headline and its page number on index pages.
- Invalid story pages (beyond X5) are now skipped, so page rotation advances to the next category block instead of wrapping back.
- Subpage counter is only drawn on story pages whose content overflows its allotted rows.

## [0.4.3] - 2025-04-16
### Changed
- Enforced strict Teletext grid alignment in **TeletextContent** and **Renderer**: every character is drawn cell-by-cell to match the 40 × 25 layout.
- Integrated **TeletextGrid** for dynamic cell sizing.
- Added automatic **subpage rotation** every 10 s and page rotation logic in **MainForm**.
- Extended **Renderer** with a `PageNumber` property and branching to static/story/RSS content.

## [0.4.2] - 2025-04-16
### Changed
- Moved debug “Story of Teletext” page from 100 to 777. The static story now only appears on page 777 when `DebugStaticStoryEnabled` is true.

## [0.4.1] - 2025-04-16
### Added
- `ConfigManager.DebugStaticStoryEnabled` toggle (via App.config) to switch header to “Story of Teletext” for debug mode.
- **TeletextContent** reads that flag and displays the appropriate 3-line header.

## [0.4.0] - 2025-04-16
### Added
- **RSS cache manager** (`RssCacheManager`) for 15 min caching of CBC RSS feeds.
- **TeletextRSSContent** to render RSS‐based Teletext pages (300–309, 310–319, etc.) with index and story pages.
- Extended **Renderer** to delegate RSS pages to `TeletextRSSContent` and static pages to `TeletextContent`.

## [0.3.8] - 2025-04-16
### Changed
- Updated **TeletextContent** rendering to enforce strict grid alignment for story content by drawing each character individually.
  - Ensures all text (header + content) adheres to the Teletext/Modeseven grid layout.

## [0.3.7] - 2025-04-16
### Changed
- Updated **TeletextContent** to reserve an extra blank row at the bottom for footer breathing room and maintain consistent left/right padding.

## [0.3.5] - 2025-04-16
### Added
- Automatic **subpage rotation** feature in **TeletextContent**:
  - Subpages cycle every 10 s.
  - **MainForm**’s timer triggers subpage advancement.

## [0.3.4] - 2025-04-16
### Changed
- Introduced a fixed Teletext grid layout (40 × 25) via **TeletextGrid**.
- Updated **Renderer** and **TeletextFooter** to calculate and respect dynamic cell sizes for any window dimension.

## [0.3.3] - 2025-04-16
### Changed
- Added a timer in **MainForm** to refresh the UI every second, ensuring the header timestamp updates in real time.

## [0.3.2] - 2025-04-16
### Fixed
- (initial bugfixes and minor tweaks)
