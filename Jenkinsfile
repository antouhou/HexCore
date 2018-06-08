pipeline {

    agent { docker { image 'mono' } }

    stages {
        stage('Install Dependencies') {
            steps {
		sh 'export HOME=$PWD'
                sh 'nuget restore'
            }
        }
        stage('Build') {
            steps {
                sh 'msbuild'
            }
        }
        stage('Test') {
            steps {
                sh 'echo "Tests should be there"'
            }
        }
    }
}
