![iam policies](./readme.artwork/iam%20policies.png)
 
![iam create policy](./readme.artwork/iam%20create%20policy.png) 

![iam policy permissions](./readme.artwork/iam%20policy%20permissions.png)

Place this in editor:

{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "route53:ListHostedZones",
                "route53:GetHostedZone",
                "route53:ChangeResourceRecordSets",
                "route53:ListResourceRecordSets"
            ],
            "Resource": "*"
        }
    ]
}

 
![iam policy editor](./readme.artwork/iam%20policy%20editor.png)
 
![iam policy editor review](./readme.artwork/iam%20policy%20editor%20review.png)

![iam policy editor create](./readme.artwork/iam%20policy%20editor%20create.png) 



![iam users](./readme.artwork/iam%20users.png) 


Click on user

“Check” the permission we gave it before, and click on “Remove”

Click on “Add permissions”, and click  on “add permissions” in drop down

 
![iam user attach](./readme.artwork/iam%20user%20attach.png)

Search for dns_management, if it does not show up click on the refresh button

Check it.

Click next

Click “Add permissions”
