# Contributing

Thank you for your interest in contributing.

Please first discuss the change you wish to make via issue/request, email, or any other method with the owners of this repository before making changes. Failure to do so will likely lead to a declined pull request regardless of quality.

Make sure to refer to and follow the [code of conduct](CODE_OF_CONDUCT.md).

## Quality Requirements

* No AI-generated LLM code contributions.
* No external dependencies.
* Digitally signed (see DCO below).
* 100% test coverage.
* Proper comments.
* Updated documentation.
* Follow the project code style.
* No #region.

Pull requests will be reviewed thoroughly to maintain project quality.

Note the ban on LLM-generated code. Developers do not have the right to submit LLM-generated code because that code can be incompatibly copyrighted or licensed elsewhere. Review the Developer Certificate of Origin adherence requirements below.

## Mac Environment Recommended Setup

1) Install Command Line Developer Tools via the Terminal:
    A) Check if installed: ‘xcode-select -p’
    B) If not, run: ‘code-select —install’
    C) Verify git works: ‘git —version’
2) Install [VS Code](https://code.visualstudio.com/docs/setup/mac).
3) Clone the repository in the editor, then:
    A) Install all recommended extensions for the project.
    B) Follow all prompts for signing in/downloads.
    C) To see build options, run from terminal: "./build/run.sh --help"
    D) Verify project state by running from terminal: "./build/run.sh test"

## Linux Environment Recommended Setup

1) Install packages for:
    A) [.NET frameworks](https://dotnet.microsoft.com/download)
    B) [Mono](https://www.mono-project.com/download/stable/#download-lin)
    C) [VS Code](https://code.visualstudio.com/docs/setup/linux)
2) Clone the repository in the editor, then:
    A) Install all recommended extensions for the project.
    B) Run from terminal: "chmod +x ./build/run.sh"
    C) To see build options, run from terminal: "./build/run.sh --help"
    D) Verify project state by running from terminal: "./build/run.sh test"

## Windows Environment Optional Setup

VS Code is still recommended (see above setups), but VS Community may be utilized instead:

1) Install [VS Community](https://visualstudio.microsoft.com/vs/community).
2) Clone the repository locally.
3) To see build options, run from cmd prompt: "./build/run.cmd --help"
4) Verify project state by running from cmd prompt: "./build/run.cmd test"

# Git & GPG Quick Setup Example

Set the following git configuration settings, replacing [xxx] with valid personal values:

1) git config --global user.name "[xxx]"
2) git config --global user.email "[xxx]@users.noreply.github.com"
3) git config --global user.signingKey [xxx]
4) git config --global tag.gpgSign true
5) git config --global commit.gpgSign true
6) git config --global push.gpgSign "if-asked"

Read more about GPG keys in the following section.

## Developer Certificate of Origin (DCO)

To allow contributions to be used by the project, developers must declare that they can and do give rights to use that code. This is done by signing off on commits by appending a message at the end of the commit for all owners in the format:

```
Signed-off-by: FirstName LastName <email@mail.com>
```

This can be accomplished manually or by using -s or --signoff when committing. Github can verify this signature with your GPG key, which you can learn about [here](https://help.github.com/articles/signing-commits-with-gpg/). By signing off your commits to this repository, you're confirming that you've read DCO and agree with it, which can be found at https://developercertificate.org/ or read below:


```
Developer's Certificate of Origin 1.1

By making a contribution to this project, I certify that:

(a) The contribution was created in whole or in part by me and I
    have the right to submit it under the open source license
    indicated in the file; or

(b) The contribution is based upon previous work that, to the best
    of my knowledge, is covered under an appropriate open source
    license and I have the right under that license to submit that
    work with modifications, whether created in whole or in part
    by me, under the same open source license (unless I am
    permitted to submit under a different license), as indicated
    in the file; or

(c) The contribution was provided directly to me by some other
    person who certified (a), (b) or (c) and I have not modified
    it.

(d) I understand and agree that this project and the contribution
    are public and that a record of the contribution (including all
    personal information I submit with it, including my sign-off) is
    maintained indefinitely and may be redistributed consistent with
    this project or the open source license(s) involved.
```
