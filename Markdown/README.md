# Telefact

**Telefact** is a retro-inspired Teletext/Ceefax project written in C#. It aims to recreate the nostalgic experience of Teletext services, adhering to Teletext 2 standards. The project is modular, highly customizable, and designed for Windows Forms.

## Current Features (Version 0.3.0)
- **Teletext Rendering**: Basic static page rendering with Teletext-appropriate colors and fonts.
- **Header and Footer Rendering**:
  - **Header**: Displays fixed elements such as a left page number (" P100"), service name ("Telefact") with a red background and yellow text, a right page number ("100"), and a timestamp (formatted as "MMM dd HH:mm:ss").
  - **Footer**: Displays a footer line with row information; uses a grid-based layout with white background for content cells and black padding on the right.
- **Modular Architecture**: Components are separated into distinct classes for easy future expansion.

## Planned Future Features
- **RSS Feed Integration**: Fetch and display news content from CBC RSS feeds.
- **Broadcast Mode**: Loop through pages with background music, mimicking "Pages from Ceefax."
- **Regional Support**: Display region-specific content.
- **Weather Information**: Local weather updates via API integration.
- **Localization**: Support for multiple languages, including French.

### Getting Started

#### Prerequisites
- .NET Framework 4.8
- Visual Studio 2022
- Basic knowledge of C# and Windows Forms

#### Installation
1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Restore NuGet packages.
4. Build and run the project.

### Folder Structure
- `\assets\fonts\`: Custom fonts (default: "Modeseven").
- `\src\`: Source code for the project.
- `\docs\`: Documentation and wiki content.
- `CHANGELOG.md`: Log of changes and releases.
- `ROADMAP.md`: Project development milestones.

### Contributing
Contributions are welcome! Please follow the roadmap and adhere to the coding and commit guidelines.

### License
This project is licensed under the MIT License. See `LICENSE` for details.
