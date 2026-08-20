# Windows QA Handoff

Use the development Mono build in `Builds/WindowsMono/` on a Windows x64 machine. This is an evidence-collection checklist, not a release approval.

1. Copy the entire `WindowsMono` directory; do not move only the `.exe`.
2. Start `UnityFoundationValidation.exe`; preserve the generated `Player.log`.
3. Press Space twice. Confirm the cube alternates between gray and cyan, then restart once and confirm the last state loads.
4. Capture one startup screenshot, one active-state screenshot, and a 10–20 second clip showing the two toggles.
5. Record Windows version, GPU, resolution, controller connected/not connected, and any warnings, crash, or missing-input behavior.
6. Attach the raw log, screenshots, and clip to the future validation evidence before calling Windows QA passed.

Windows IL2CPP is intentionally out of scope for this lab. It becomes a separate release-game gate on Windows hardware.
