# Run tests and generate coverage
NUNIT_RESULTS=nunit-results.xml
PROFILER_PROFILE=log:coverage,covfilter-file=coverage.filter
TESTS_EXECUTABLE=Tests/bin/Debug/Tests.exe
mono --debug --profile=$PROFILER_PROFILE $TESTS_EXECUTABLE --result=$NUNIT_RESULTS

# Transform results to junit format
saxonb-xslt $NUNIT_RESULTS nunit3-junit.xslt > junit-results.xml

# Saving coverage output:
mprof-report --reports=coverage --out=coverage.report output.mlpd
