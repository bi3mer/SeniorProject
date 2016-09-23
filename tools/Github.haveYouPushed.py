import pprint
pp = pprint.PrettyPrinter(indent=4)

import git

repo = git.Repo('../.')

branches = repo.branches

commits = {}

for commit in repo.iter_commits("master"):
    author = commit.author.name
    date   = commit.committed_datetime.strftime("%a %d. %b %Y")
    
    if author not in commits:
        commits[author] = date

pp.pprint(commits)
