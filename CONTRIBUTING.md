# How to contribute

- Suggesting changes or new features via [issues](https://github.com/zeplar-exe/BakedEnv/issues) that adhere to the guidelines below.
- Reporting bugs or quirks via [issues](https://github.com/zeplar-exe/BakedEnv/issues) that adhere to the guidelines below.
- Submitting [pull requests](https://github.com/zeplar-exe/BakedEnv/pulls) that adhere to the guidelines below.

## Issues

Issues should:

- <details>
    <summary>Be a bug, feature, or question</summary>
    
    > Discussions or otherwise non-issues should be directed to discussions. Questions are also better
    suited for discussions.
</details>

- <details>
    <summary>Have a well-defined scope</summary>
    
    > Similar to StackOverflow, an issue should be focused on one bug, feature, or question. Split them
    into multiple issues when possible.
</details>

- <details>
    <summary>Be descriptive</summary>
    
    > It is strongly reccomended that issues abide by our issue templates. Otherwise, an issue should
    contain any relevant information, such as the referenced code, logs (if possible), and stack traces.
</details>

### Issue Branches

Branches tied to development for a certain issue are to be named "\<username\>/issue/<issue-number\>". 
  
## Pull Requests

In almost every case, pull requests should be connected to a specific issue. It is strongly reccomended 
that your changes do not exceed the scope of the target issue. This is evident when unrelated systems 
are changed, and should discussed in a separate issue.

### Testing

Unit tests should reflect any changes made in pull requests. This includes source generators to a lesser 
extent, where the tested code is very volatile.

### Documentation

Modification and creation of features should be reflected in the [documentation](https://github.com/zeplar-exe/BakedEnv/tree/docfx/docfx_project).
