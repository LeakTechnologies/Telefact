<!-- ROADMAP.md -->
# Telefact Roadmap

This document tracks both completed milestones and upcoming goals.

---

## ✅ Completed

### 0.1.0 – Initial Prototype
- Basic Windows Forms setup.
- Draft Teletext grid constants (40×25).

### 0.2.0 – Static Teletext Content
- Implemented static `TeletextContent` rendering.
- Header & footer drawn with Modeseven font.
- Basic page-flip support.

### 0.3.0 – Subpage Rotation & Grid Layout
- Introduced `TeletextGrid` (40 × 25).
- Dynamic `cellWidth`/`cellHeight` based on window size.
- Automatic subpage rotation (10 s interval).

### 0.4.0 – RSS Integration Part 1
- `RssCacheManager` caches CBC RSS feeds (15 min).
- `TeletextRSSContent` renders index (X00…X09) and story pages.
- Extended `Renderer` to branch between static and RSS pages.

### 0.4.1 / 0.4.2 – Debug Static Story Mode
- `ConfigManager.DebugStaticStoryEnabled` toggles page 777 “Story of Teletext.”
- Moved static story page from 100 to 777.

### 0.4.3 – Strict Grid Alignment & Enhanced RSS UX
- Enforced per-character grid rendering on all RSS pages.
- Index pages: 5 headlines, dot-leader navigation, breathing rows.
- Story pages: uppercase titles (white), original-case bodies (cyan), robust wrapping & HTML stripping.

---

## 🔜 Upcoming

### 0.5.0 – Broadcast & Presentation Mode
- Full “Pages from Ceefax” loop mode.
- Background audio (beeps, jingles).
- On-screen progress indicator (page timer).

### 0.6.0 – Double-Height Titles & Styling
- Support a separate double-height title font/asset.
- Enhanced color palette for subheadings.

### 0.7.0 – Regional & Custom Pages
- Allow users to pick regional news feeds.
- Plugin interface for adding new data sources (weather, stocks, sports).

### 0.8.0 – Weather & Localization
- Integrate local weather widgets (API-based).
- Full bilingual support (EN/FR) and date‐locale formatting.

### 1.0.0 – Public Release
- Polish, performance tuning, packaging.
- Full documentation, samples, and installer.

---

### 📌 Longer-Term Ideas
- Accessibility modes (high-contrast, font scaling).
- User-customizable page list and timing via UI.
- Docker headless mode + web-based remote control.

