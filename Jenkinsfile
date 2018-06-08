pipeline {
    agent { docker { image 'mono' } }
    stages {
        stage('Install Dependencies') {
            steps {
                sh 'nuget install'
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
