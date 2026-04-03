# Telefact Agent Workflow Rules

These rules apply to any automation or agent working in this repo.

## Current Project State

- Current version: `v0.4.5`.
- Public/stable baseline: `v0.4.5`.
- Primary planning source is `Markdown/ROADMAP.md`; release-facing history is `Markdown/CHANGELOG.md`.
- Issue tracker: `https://git.leaktechnologies.dev/leak_technologies/Telefact/issues`.
- No active TODO.md or DONE.md yet — all scope is tracked in ROADMAP.md.

## Immediate Handoff Priorities

- **v0.5.0 – Broadcast & Presentation Mode** — Implement full "Pages from Ceefax" loop mode, background audio (beeps/jingles), and on-screen page timer/progress indicator.
- **Double-height titles** — Groundwork: confirm font asset approach before v0.6.0 work begins.
- **Issue tracker** — Open issues for any discovered bugs during v0.5.0 work.
- Do not expand scope beyond what is listed unless explicitly approved.
- Keep the issue tracker in sync — close issues when work lands, open new ones for discovered bugs.

## Commit Discipline

- After every change: `git add` then `git commit -m "..."`.
- Do not leave unstaged changes in the worktree.
- Commit only files related to the current task.
- Use conventional commit prefixes: `feat:`, `fix:`, `docs:`, `refactor:`, `chore:`.

## Documentation Discipline

- Primary docs live in `Markdown/`:
  - `Markdown/ROADMAP.md` — milestone tracking (upcoming and completed)
  - `Markdown/CHANGELOG.md` — release-facing history
  - `Markdown/README.md` — project overview and getting started
- Always update `Markdown/CHANGELOG.md` and `Markdown/ROADMAP.md` when completing or planning work.
- Avoid personal names in documentation; use `user report` or `dev report` only.

### New Feature Documentation

- Before implementing a significant feature, create `Markdown/FEATURE_NAME.md`.
- Include: overview, technical design, files to modify, and a testing checklist.
- Link the design doc in ROADMAP.md and in this file's Immediate Handoff Priorities.
- Example naming: `Markdown/BROADCAST_MODE_DESIGN.md`, `Markdown/DOUBLE_HEIGHT_DESIGN.md`.

## Version Bumping

- After every significant feature or fix: bump the version string.
- Version appears in `AssemblyInfo.cs` (`AssemblyVersion`, `AssemblyFileVersion`).
- Versioning model:
  - `v0.x.y` is the rolling development line.
  - Milestone releases (v0.5.0, v0.6.0, etc.) mark completed roadmap milestones.
- After bumping: update `Markdown/CHANGELOG.md` and `Markdown/ROADMAP.md`.

## Platform Scope

Telefact targets **Windows only** (Windows Forms / .NET Framework 4.8).

- Do not add cross-platform code paths, Linux-specific logic, or macOS conditionals.
- The runtime is .NET Framework 4.8 — do not upgrade to .NET 5+ without explicit approval.
- NuGet dependencies must be compatible with .NET Framework 4.8.
- Currently: `System.ServiceModel.Syndication 4.6.0` for RSS parsing. Do not add heavy dependencies without discussion.

## Architecture & Code Conventions

### Class responsibilities (do not blur these boundaries)

| Class | Role |
|---|---|
| `MainForm.cs` | Windows Forms shell, keyboard navigation, timer wiring only |
| `Renderer.cs` | Routes a page number to the correct content renderer |
| `TeletextHeader.cs` | Draws the top header row |
| `TeletextFooter.cs` | Draws the bottom footer row |
| `TeletextContent.cs` | Renders static Teletext pages |
| `TeletextRSSContent.cs` | Renders live RSS-backed pages (300–399) |
| `RSSFeedParser.cs` | Parses CBC RSS feeds into story objects |
| `RSSCacheManager.cs` | 15-minute cache layer over `RSSFeedParser` |
| `ConfigManager.cs` | Reads `App.config` values |

- New features that add a distinct content source or rendering mode should get their own class, following the `TeletextRSSContent` pattern.
- Keep `MainForm.cs` thin — no business logic or rendering logic there.

### Grid & rendering rules

- The canonical grid is **40 columns × 25 rows**.
- All content (header, footer, body) must be drawn **character-by-character** on the grid.
- Cell dimensions (`cellWidth`, `cellHeight`) are dynamic — always derive from the current window size via `TeletextGrid`; never hardcode pixel values.
- RSS pages live in the **300–399** range. Static pages are outside that range.
- Page 777 is reserved for the debug "Story of Teletext" mode (controlled by `DebugStaticStoryEnabled` in `App.config`).

### App.config flags

| Key | Purpose |
|---|---|
| `DebugStaticStoryEnabled` | When `true`, page 777 shows the static story debug mode |

Add new debug/config flags here, not as hardcoded constants.

## Testing

- There is currently no automated test suite.
- Manual smoke test after every change:
  1. Build succeeds with no warnings promoted to errors.
  2. App launches and displays page 100 with correct header/footer.
  3. Subpage rotation advances after 10 seconds.
  4. RSS pages (e.g. 300) load and render within 15 seconds.
  5. Page navigation (keyboard) moves between pages correctly.
- When adding a new feature, include a testing checklist in its design doc.

## Using Sub-Agents

When multiple independent tasks exist within a single change, use sub-agents to parallelize work.

### When to use sub-agents

- Multiple similar fixes across several files.
- Independent tasks that do not share state.
- Larger features that can be split into logical, non-overlapping chunks.

### How to use

```
Agent(description="Implement broadcast loop timer", prompt="...", subagent_type="general-purpose")
Agent(description="Implement audio jingle player", prompt="...", subagent_type="general-purpose")
```

### Guidelines

- Provide clear, specific instructions including file paths and class names.
- Include the expected commit message in the prompt.
- Verify the build passes after all sub-agents complete.
- Resolve any merge conflicts manually before committing.

## Coordination

- Ask before changing `App.config` schema or adding new required config keys.
- Ask before adding new NuGet package dependencies.
- Keep `Markdown/ROADMAP.md` as the single source of truth for what is planned vs. complete.
- Do not move completed milestone items back to "Upcoming" — they are permanent record.
