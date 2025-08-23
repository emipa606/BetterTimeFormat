# GitHub Copilot Instructions for Better Time Format (Continued)

## Mod Overview and Purpose

**Better Time Format (Continued)** is a RimWorld mod that enhances the in-game clock display to provide players with customized time formats. The original format of "18h" is transformed into more intuitive and satisfying layouts like "18:05" or "12am". It is designed to utilize the 12-hour clock setting directly from game preferences, making it more seamlessly integrated into the game. This mod is compatible with RimWorld versions 1.2 and up.

## Key Features and Systems

- **Customizable Time Formats**: Allows players to define custom time formats using simple placeholders. For example, the format "HH:MM" could display as "07:15".
- **Real-Time Clock with Seconds**: Offers the possibility to display minutes and seconds, letting players see time zoom past during accelerated gameplay.
- **Easy Integration and Management**: Safe to add and remove from savegames without any issues.
- **Performance**: Although the time formatting logic prioritizes performance over absolute accuracy, it generally works fine for the majority of use cases.

## Coding Patterns and Conventions

- The codebase uses a blend of best practices in C#, adhering to general naming conventions like PascalCasing for class names and method names.
- Code is structured into classes that represent different aspects of the mod, for instance:
  - `BetterTimeFormat` (Utility class for formatting)
  - `BetterTimeFormatMod` (Main mod class)
  - `BetterTimeFormatSettings` (Settings management)
  - `DateReadout_DateOnGUI` (Modifies the UI drawing for time)

## XML Integration

- While the primary logic of the mod is implemented in C#, XML integration remains critical for loading in-game configurations.
- Mod settings are configurable via XML, ensuring that players can adjust their preferred time format easily through the mod's settings menu.

## Harmony Patching

- Harmony is used to inject custom logic into RimWorld's existing methods, providing extensibility and seamless integration.
- A specific patch modifies the date readout rendering method to display time in custom formats defined by the user.
- It is essential that every patch is applied and removed correctly to ensure the smooth functioning of both the modded gameplay and potential updates.

## Suggestions for Copilot

- **Automate UI Integration**: Develop code snippets that automatically integrate new UI logic for better handling of player-defined time settings.
- **Simplify Time Parsing Methods**: Propose optimized methods to parse and format the player-defined time strings to maintain performance.
- **Enhance Harmony Patches**: Offer suggestions for applying Harmony patches that maximize compatibility with future versions of RimWorld.
- **Error Handling**: Generate robust error-handling code to manage unexpected input formats gracefully without degrading game performance.
- **Document Code Efficiently**: Help generate comments and documentation to keep the codebase understandable for future contributors or users who choose to fork the mod.

By adhering to these guidelines and leveraging existing code structures, Copilot can help maintain and expand the Better Time Format mod efficiently.
