<!-- README.md -->
# Telefact

**Telefact** is a retro‐inspired Teletext/Ceefax project written in C#. It aims to recreate the nostalgic experience of Teletext services, adhering to Teletext 2 standards. The project is modular, highly customizable, and designed for Windows Forms.

## Current Features (Version 0.4.3)
- **Teletext Rendering**: Basic static page rendering with Teletext-appropriate colors and Modeseven font.
- **Header and Footer Rendering**:
  - **Header**: Displays fixed elements such as a left page number (" P100"), service name ("Telefact") with a red background and yellow text, a right page number ("100"), and a timestamp (formatted as "MMM dd HH:mm:ss").
  - **Footer**: Displays a footer line with row information; uses a grid‐based layout with white background for content cells and black padding on the right.
- **Strict Grid Alignment**: All text (header, static content, RSS content) is drawn character-by-character on a 40×25 cell grid to eliminate kerning drift.
- **Modular Architecture**: Components are separated into distinct classes (`Renderer`, `TeletextHeader`, `TeletextFooter`, `TeletextContent`, `TeletextRSSContent`, etc.) for easy future expansion.
- **RSS Feed Integration**:  
  - Fetch and cache CBC RSS feeds every 15 minutes via `RssCacheManager`.  
  - Render index and story pages in the 300–399 range with `TeletextRSSContent`.  
  - Index pages list 5 headlines with dot-leaders and direct page links; story pages show uppercase titles (white) and original-case bodies (cyan).
- **Subpage & Page Rotation**:  
  - Automatic per-section RSS page rotation every 10 seconds.  
  - Static “Story of Teletext” page (page 777) rotates through subpages when debug‐flag is enabled.
  
## Planned Future Features
- **Broadcast Mode**: Loop through pages with background audio, mimicking “Pages from Ceefax.”
- **Double-Height Titles**: Support a separate title font with double-height characters.
- **Regional Support**: Display region-specific content (e.g. local headlines, menus).
- **Weather Integration**: Local weather updates via a public API.
- **Localization**: Full French and other language support (menus, dates, content).
- **User Configuration UI**: Allow end-users to select which pages/categories to include and set intervals.
- **Plugin System**: Let third-party modules add new data sources (stocks, sports, etc.).

### Getting Started

#### Prerequisites
- .NET Framework 4.8  
- Visual Studio 2022  
- Basic knowledge of C# and Windows Forms

#### Installation
1. Clone the repository.  
2. Open `Telefact.sln` in Visual Studio 2022.  
3. Restore NuGet packages.  
4. Build and run the project.

### Folder Structure
- `assets/fonts/`: Custom fonts (default: “Modeseven”).  
- `src/`: Source code for the project.  
- `docs/`: Documentation and wiki content.  
- `CHANGELOG.md`: Log of changes and releases.  
- `ROADMAP.md`: Project development milestones.

### Contributing
Contributions are welcome! Please follow the roadmap and adhere to the coding and commit guidelines.

### License
This project is licensed under the MIT License. See `LICENSE` for details.
