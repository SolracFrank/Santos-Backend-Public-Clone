# Release Checklist

- Run the build, test suite, dependency audit, and secret scan.
- Test with MySQL and the client in staging. Check attachment permissions, session revocation, and concurrent payment allocations.
- If a credential was exposed, rotate it and check logs and telemetry for copies. Keep secrets and production data out of the release.
- Confirm the rights holders approve the material included in any public release.
