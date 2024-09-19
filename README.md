# StarterKit.Maui Project Structure

The project is organized into several main folders, each serving a specific purpose in the application's architecture.

## Project Structure

- `src`: Root directory for source code.
  - `Configurations`: Contains configuration files and settings.
  - `StarterKit.Maui.App`: The main application project.
    - `Container`: Dependency injection container setup.
    - `Database`: Database access and management.
    - `Platforms`: Platform-specific implementations.
    - `Resources`: Application resources like images and styles.
  - `StarterKit.Maui.Common`: Common utilities and helpers.
    - `Constants`: Application-wide constants.
    - `Localization`: Localization resources and helpers.
    - `Utilities`: Common utility classes and methods.
  - `StarterKit.Maui.Core`: Core application logic.
    - `Data`: Data access layer.
    - `Domain`: Domain models and business logic.
    - `Infrastructure`: Infrastructure services like platform and environments.
    - `Presentation`: Presentation logic and view models.
  - `StarterKit.Maui.Features`: Feature modules.
    - `Feature1`: An example feature module.
      - `Data`: Data access for the feature.
      - `Domain`: Domain logic for the feature.
      - `Presentation`: Presentation and view models for the feature.
  - `StarterKit.Maui.Features.UnitTests`: Unit tests for feature modules.
    - `Feature1`: Unit tests for Feature1.
      - `Data`: Tests for data access layer.
      - `Domain`: Tests for domain logic.
      - `Presentation`: Tests for presentation logic.

## Overview

The `StarterKit.Maui` project is structured to support modularity, separation of concerns, and ease of testing. Each layer of the application is clearly separated into its own directory, allowing for independent development and testing of each aspect of the application.