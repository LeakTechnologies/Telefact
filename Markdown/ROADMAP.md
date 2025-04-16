# Telefact Project Roadmap

This document outlines the development roadmap for the "Telefact" project, versioned incrementally. Each version represents a significant milestone in the development process. Regular git commits will be made for every feature that is implemented and works without errors.

---

## **0.1.0 - Initialize Project** [Completed]
- Set up the project structure using Windows Forms.
- Add necessary dependencies:
  - `System.ServiceModel.Syndication` or `CodeHollow.FeedReader` for RSS parsing.
  - `NAudio` for audio playback.
  - `System.Runtime.Caching` for caching.
- Create a basic `TeletextRenderer` class to render a static Teletext page with:
  - Black background.
  - Teletext-appropriate colors.
  - Grid-based layout.

---

## **0.2.0 - UI Rendering** [Completed]
- Implement the `TeletextRenderer` class to render content to a grid-based layout.
- Ensure the layout mimics a BBC Modeseven terminal.
- Add support for custom fonts:
  - Place fonts in an `\assets\fonts\` folder.
  - Use "Modeseven" as the default font.
- Render a sample static page to verify the UI.

---

## **0.3.0 - Header and Footer** [Completed]
- Create `TeletextHeader` as a protected resource.
  - Render fixed header components: left page number (" P100"), service name ("Telefact"), right page number ("100"), and timestamp.
- Create `TeletextFooter` to render footer information with proper cell grid layout.
- **Important**: Header layout is finalized and must not be modified without explicit permission.

---

## **0.4.0 - RSS Feed Parsing**
- Implement the `RssFeedParser` class to fetch and parse CBC RSS feeds.
- Create a `CacheManager` to store parsed RSS content.
- Set up a 15-minute interval for automatic RSS feed updates.
- Add error handling for network issues and invalid RSS feeds.

---

## **0.5.0 - Basic Broadcast Mode**
- Implement the `BroadcastMode` class to display pages in a loop.
- Add a timer to transition between pages every 10 seconds.
- Integrate background music playback using `NAudio`.

---

## **0.6.0 - Regional Support**
- Implement a `ConfigManager` to handle regional settings.
- Allow users to select their CBC region via a configuration file.
- Fetch and display region-specific RSS feeds based on the selected region.

---

## **0.7.0 - Teletext Standards Compliance**
- Add support for subpages, fast text links, and clock display.
- Ensure all pages adhere to Teletext 2 standards for layout and navigation.

---

## **0.8.0 - Weather Information**
- Integrate a weather API (e.g., OpenWeatherMap) to fetch local weather data.
- Display weather information on a dedicated Teletext page.

---

## **0.9.0 - Localization**
- Add support for Radio-Canada RSS feeds and French language content.
- Use resource files for localization to support multiple languages.

---

## Future Versions
- Explore additional features such as user-customizable themes, advanced navigation, and more.
- Continue refining the project based on user feedback and new requirements.

---

### Git Commit Guidelines
- Commit each feature or milestone as soon as it is implemented and works without errors.
- Use clear and descriptive commit messages, e.g.:
  - `feat: Add basic TeletextRenderer with static page rendering`
  - `fix: Resolve caching issue in RssFeedParser`
  - `chore: Update dependencies for multi-platform support`
