# ✨ Changelog (`v2.15.6`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v2.15.6
Previous version ---- v2.15.5
Initial version ----- v2.12.10
Total commits ------- 1
```

## [v2.15.6] - 2026-09-04

### :arrows_counterclockwise: Changed

- Allow `Index` (candidate number) to be persisted on candidate updates so candidate numbers are now correctly saved after drag-and-drop reordering and after reload, the updated candidate positions and numbers are retained instead of reverting to original values (VE-1975)

### :x: Removed

- Removed `Index` property from `IgnoredModifyProperties` in CandidateRepository

## [v2.15.5] - 2026-08-28

### ⚠️ Deprecated

- remove deprecated notify oauth 2.0 scope

## [v2.15.4] - 2026-08-17

### 🔄 Changed

- migrate from secure connect notify service to standard voting email notification service

### ❌ Removed

- secure connect notify service integration

## [v2.15.3] - 2026-08-13

### 🔄 Changed

- use eCH-0157 from Voting.Lib

## [v2.15.2] - 2026-07-03

### 🔄 Changed

- extend e-mail recipient logging in notification service

## [v2.15.1] - 2026-06-17

### 🔄 Changed

- correctly export dwelling address in eCH-export

## [v2.15.0] - 2026-06-17

### 🆕 Added

- add country to candidates

## [v2.14.11] - 2026-06-10

### 🔄 Changed

- correctly export candidate address in eCH export

## [v2.14.10] - 2026-06-09

### 🔄 Changed

- replace per-tenant calls when fetching users from child tenants

## [v2.14.9] - 2026-06-05

### 🔄 Changed

- update V1Application cache duration

## [v2.14.8] - 2026-06-05

### 🔄 Changed

- parallelize user calls for child tenants from permission service

## [v2.14.7] - 2026-05-07

### 🔄 Changed

- check election read access when fetching ballot document

## [v2.14.6] - 2026-05-07

### 🆕 Added

- add license key registration for AutoMapper

### 🔄 Changed

- bump AutoMapper from v12.0.1 to v15.1.3

## [v2.14.5] - 2026-04-22

### 🔄 Changed

- preserve list union connections on list update

## [v2.14.4] - 2026-04-15

### 🔄 Changed

- remove my account

## [v2.14.3] - 2026-03-25

### 🔄 Changed

- leave unmodified candidates as-is

## [v2.14.2] - 2026-03-18

### 🔄 Changed

- remember candidate created info correctly

## [v2.14.1] - 2026-03-18

### 🔒 Security

- ensure election and list id match when updating/deleting comments

## [v2.14.0] - 2026-03-04

### 🔄 Changed

- require one candidate per list

## [v2.13.0] - 2026-03-04

### 🔄 Changed

- remove majority election candidate number from exports

## [v2.12.21] - 2026-02-27

### 🆕 Added

- integration tests for all controller methods that enforce the election archive guard

### 🔒 Security

- refactor election archive guard
- prevent unauthorized manipulation of archived elections

## [v2.12.20] - 2026-02-18

### 🔄 Changed

- reorder metrics middleware calls in Startup configuration to catch final response status.

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
