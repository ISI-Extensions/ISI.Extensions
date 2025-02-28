https://stackoverflow.com/questions/10054318/how-do-i-provide-a-username-and-password-when-running-git-clone-gitremote-git/10054470#10054470

git config --global url."https://ssh:$MY_GIT_TOKEN@github.com/".insteadOf "ssh://git@github.com/"

git config --global --remove-section url."git@github.com"
