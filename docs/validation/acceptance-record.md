# Acceptance Record

Recorded: 2026-08-20

## Verified locally

| Check | Result | Evidence |
| --- | --- | --- |
| Unity project baseline | Pass | Unity `6000.5.9f1`, URP `17.5.0`, Input System `1.20.0`, Test Framework `1.7.0`, UGUI `2.5.0`, and Visual Studio integration `2.0.26` resolved in `Packages/manifest.json` and `packages-lock.json`. |
| Source-control configuration | Pass | Force Text, Visible Meta Files, local Git repository, Git LFS filters, tracked configuration and ignored generated folders. A separate LFS clean-clone round-trip is recorded after the initial remote push. |
| Assembly boundaries | Pass | `Shared → Signal → UI → Bootstrap`; Editor and test assemblies are leaf consumers. The independent code audit found no production device polling, service locator, global event bus, or feature-to-sibling dependency. |
| EditMode tests | Pass | 4/4 passed at `2026-08-20 16:29:45Z`; raw result is local `TestResults/editmode-results.xml`. |
| PlayMode input path | Pass | 1/1 passed at `2026-08-20 16:31:30Z`; the test simulates Space through the Input System and asserts both runtime state change and presenter-state agreement. Raw result is local `TestResults/playmode-results.xml`. |
| Windows Mono development build | Pass | Unity reported success in `Logs/windows-mono-build.log`. `Builds/WindowsMono/UnityFoundationValidation.exe` is a PE32+ x86-64 executable; SHA-256 `da7f9558bf80ab2b695db04a76773c3ee62eaabf9c367d02f44d9270e483bfab`. |
| VS Code project generation | Pass | The per-assembly `.csproj` files and `.slnx` were generated and the workspace opens with the configured Unity attach launch profile. |

## Not verified, by design

- VS Code debugger attachment and Unity Console links need a visible Editor session.
- A Windows host must run the build and provide log, screenshots, and a clip using `windows-qa-checklist.md`.
- Runtime performance profiling, Windows IL2CPP, private remote/LFS clean clone, and Unity AI beta sandbox each remain separate gates.
- No item above proves a real game, market, or Steam release readiness.
