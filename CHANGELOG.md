# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- Build and deploy scripts.
- Allow properties to be read using more than one header name.

### Changed
- Redefine mapping process.
- Default behavior of reading is now to exit on first resource failure.

### Removed
- `MarkHeaderASOptional` and `MarkBodyAsOptional` mapping methods.

## [0.1.1] - 2021-09-13
### Changed
- Publicize mapping options.

## [0.1.0] - 2021-09-09
### Added
- API for reading resources one-by-one.
- Integration testing.
- Mapping option validation.

### Changed
- Row number is included on results from reading operations.
- Replace internal enumeration class with "Ardalis.SmartEnum" package.

### Deprecated
- `MarkHeaderASOptional` and `MarkBodyAsOptional` mapping methods in favor of single `MarkAsOptional`.

## [0.0.3] - 2021-05-09
### Changed
- Use public records for result models from reading operations.

## [0.0.2] - 2021-05-08
### Changed
- Update dependencies to use the initial version of the current major release.

## [0.0.1] - 2021-05-08
### Added
- Initial release.

[Unreleased]: https://github.com/lanceccraig/SpreadsheetIO/compare/0.1.1...HEAD
[0.1.1]: https://github.com/lanceccraig/SpreadsheetIO/compare/0.1.0...0.1.1
[0.1.0]: https://github.com/lanceccraig/SpreadsheetIO/compare/0.0.3...0.1.0
[0.0.3]: https://github.com/lanceccraig/SpreadsheetIO/compare/0.0.2...0.0.3
[0.0.2]: https://github.com/lanceccraig/SpreadsheetIO/compare/0.0.1...0.0.2
[0.0.1]: https://github.com/lanceccraig/SpreadsheetIO/releases/tag/0.0.1
