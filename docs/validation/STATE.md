# Technical Validation Lab State

Updated: 2026-08-20

## Purpose

This repository validates a Unity 6.5 solo-game technical foundation. It is not a game project and contains no market premise, commercial art, lore, or release claim.

## Locked Baseline

- Unity `6000.5.9f1`, Universal 3D / URP
- Windows x64 Steam is the future release target; this Mac creates development Mono builds only
- Input System, Test Framework, UGUI, Visual Studio integration, Git, and Git LFS are installed
- Source-control mode is Visible Meta Files and asset serialization is Force Text
- No Web Build Support, Unity AI beta integration, paid tool, third-party asset, custom MCP server, Steamworks SDK, or GitHub remote is active

## Current Technical Slice

`Signal` is a neutral validation feature, not a game mechanic. It proves:

- authored `SignalDefinition` ScriptableObject is separate from immutable runtime `SignalSessionState`;
- a versioned `SignalSaveData` DTO uses a stable content ID;
- the Bootstrap assembly composes the save store, semantic Input System action, presenter prefab, and additive validation scene;
- one simple primitive prefab presents state without gameplay code knowing about UI.

## Evidence Completed

- Unity import/compile and scene/prefab generation: `Logs/lab-build.log`
- EditMode deterministic suite: 4 passed in `TestResults/editmode-results.xml`
- PlayMode bootstrap/presenter/semantic-action path: 1 passed in `TestResults/playmode-results.xml`
- Windows x64 development Mono build: `Builds/WindowsMono/UnityFoundationValidation.exe`

## Human Gates Still Open

- Create/approve a private GitHub remote, then commit/push and verify an LFS clean clone.
- Run the build on a Windows machine and collect log, screenshots, and a short clip.
- Open VS Code with the Unity Editor and manually confirm debugger attachment plus Console links.
- Accept Unity AI beta/cloud/credit terms with data sharing disabled before any read-only sandbox trial; approve any write separately.

## Explicit Non-Claims

This lab does not validate a real game's market, art, feel, pacing, performance on Windows hardware, IL2CPP release build, paid assets, Steam SDK, store page, or publication.
