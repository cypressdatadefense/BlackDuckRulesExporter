# BlackDuckRulesExporter

Exports Black Duck Rules

``` BASH
Usage: BlackDuckRulesExporter <blackduck-url> <api-token> [--json]
```

* You will need an API TOKEN from Black Duck to use this app.

1. From the user menu located on the top navigation bar, select Access Tokens. The Access Tokens page appears.

2. Click Create Token. The Create ruleToken dialog box appears.

3. Type a name in the Name field.

    Optional: In the Description field, type a description or definition.

4. Select Read Access and/or Write Access.

5. Click Create. The API token displays in a pop-up window. For security reasons, this is the only time your user API token displays.  Please save this token. If the token is lost, you must regenerate it.

* You can test your token with:

``` BASH
curl -X POST \ 
https://< server URL>/api/tokens/authenticate \
-H "Accept: application/vnd.blackducksoftware.user-4+json" \
-H "Authorization: token <api-token>"
```