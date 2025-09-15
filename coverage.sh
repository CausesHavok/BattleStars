#!/bin/bash

echo "🧹 Cleaning old test results and coverage reports..."
rm -rf BattleStars.Tests/TestResults BattleStars.Tests/CoverageReport CoverageReport

echo "🧪 Running tests with coverage..."
dotnet test --collect:"XPlat Code Coverage"

echo "📊 Generating coverage report from latest results..."
LATEST_COVERAGE=$(find BattleStars.Tests/TestResults -name coverage.cobertura.xml | sort | tail -n 1)
reportgenerator -reports:"$LATEST_COVERAGE" -targetdir:CoverageReport -reporttypes:Html

echo "✅ Coverage report ready at CoverageReport/index.html"
