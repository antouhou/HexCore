# Run tests and generate coverage
PROFILER_PROFILE="log:coverage,covfilter-file=coverage.filter"
TESTS_EXECUTABLE="Tests/bin/Debug/Tests.exe"
mono --debug --profile=$PROFILER_PROFILE $TESTS_EXECUTABLE --result=nunit-results.xml

# Transform results to junit format
saxonb-xslt TestResult.xml nunit3-junit.xslt > junit-results.xml

# Saving coverage output:
mprof-report --reports=coverage --out=coverage.report output.mlpd
