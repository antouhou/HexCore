pipeline {

    agent { docker { image 'mono' } }

    stages {
        stage('Install Dependencies') {
            steps {
                sh 'HOME=$PWD nuget restore'
            }
        }
        stage('Build') {
            steps {
                sh 'msbuild'
            }
        }
        stage('Test') {
            steps {
                sh 'mono packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe Tests/bin/Debug/Tests.dll
'
            }
        }
    }
}
