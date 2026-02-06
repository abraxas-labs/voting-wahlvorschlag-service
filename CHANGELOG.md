# ✨ Changelog (`v2.12.19`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v2.12.19
Previous version ---- v2.12.10
Initial version ----- v2.12.10
Total commits ------- 9
```

## [v2.12.19] - 2026-02-06

### 🔄 Changed

- extend CD pipeline with enhanced bug bounty publication workflow

## [v2.12.18] - 2026-01-16

### 🔄 Changed

- export list short description

## [v2.12.17] - 2025-12-22

### 🔄 Changed

- limit access to list (sub)-unions to election administrators only.
- Prevent leakage of list identifications through list unions between independent parties.

## [v2.12.16] - 2025-12-19

### 🔄 Changed

- update input validation range for candidate index

## [v2.12.15] - 2025-12-19

### 🔄 Changed

- add input validation for candidate index fields `Index`, `OrderIndex` and `CloneOrderIndex` to range `[1; 100]`

## [v2.12.14] - 2025-12-19

### 🔄 Changed

- extend email mapping to require verified status

## [v2.12.13] - 2025-12-18

### 🆕 Added

- added additional user service integration tests

### 🔄 Changed

- extend user detail view to show email address in user management for election administrators

## [v2.12.12] - 2025-12-05

### 🔒 Security

- prevent unauthorized manipulation of "Locked" attribute

## [v2.12.11] - 2025-10-31

### 🔄 Changed

- apply election submission deadline policy for lists

## [v2.12.10] - 2025-10-01

### 🎉 Initial release for Bug Bounty
