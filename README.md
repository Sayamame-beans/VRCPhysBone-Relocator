# VRCPhysBone-Relocator
## Summary/概要
Relocate the PhysBone and PhysBoneCollider Components to the object set in the "Root Transform".\
This is useful when the object corresponding to the PhysBone is hard to find (e.g. VRoid Avatar).

PhysBoneコンポーネント類を\"Root Transform\"に設定されているゲームオブジェクトに再配置します。\
1つのゲームオブジェクトに大量のPhysBoneコンポーネント類が含まれていて、対応するゲームオブジェクトが分かりづらい場合などに役に立ちます。(変換直後のVRoid製アバターなど)

## How to install/導入方法
### Using unitypackage file/unitypackageファイルを使った方法

TODO: update how to install to support CusomLocalization4EditorExtension

[EN]
1. Download the latest version of unitypackage from [here](https://github.com/Sayamame-beans/VRCPhysBone-Relocator/releases).
2. Import the unitypackage to your project.
(Drag&Drop or "Assets → Import → Custom Package..." at the top of the Editor window.)

[JP]
1. [ここ](https://github.com/Sayamame-beans/VRCPhysBone-Relocator/releases)から最新バージョンのunitypackageをダウンロードします。
2. プロジェクトにunitypackageをインポートします。
(ドラッグ&ドロップ、またはエディタ上部の"Assets → Import → Custom Package...")

### Using git(through Unity Package Manager)/gitを使った方法(Unity Package Manager経由)
[EN]
1. Open your unity project.
2. Open the Package Manager from "Window → Package Manager" at the top of the Editor window.
3. Click `+` at the upper left of the Package Manager window , then select `Add package from git URL...`.
4. Enter `https://github.com/Sayamame-beans/VRCPhysBone-Relocator.git` , then press "Add".

[JP]
1. Unityプロジェクトを開きます。
2. エディタ上部の"Windows → Package Manager"から、Package Managerを開きます。
3. Package Managerウィンドウの左上の`+`をクリックし、`Add package from git URL...`を選択します。
4. `https://github.com/Sayamame-beans/VRCPhysBone-Relocator.git`と入力し、"Add"を押します。

## Usage/使い方
1. Open the window from "Tools → PB Relocator" at the top of the Editor window.
2. Set the target GameObject, then press "Relocate!"

You can Undo/Redo this operation.

1. エディタ上部の"Tools → PB Relocator"から、ウィンドウを開きます。
2. 対象のゲームオブジェクトをセットし、"Relocate!"ボタンを押します。

この操作は元に戻したり、やり直したりする事が出来ます。

---
## Notes/備考
This editor extension is in beta. I will add more features in the future.\
I expect this will work properly, but please let me know if you find any problems.

このエディタ拡張はベータ版です。今後、機能を追加したりすると思います。\
正常に動作するとは思いますが、不具合があった場合はお知らせください。

Link:
- [GitHub](https://github.com/Sayamame-beans/VRCPhysBone-Relocator)
- [Booth](https://sayamame-beans.booth.pm/items/3872837)

## To Build this project

clone this repository into `Packages` folder in your project.

In addition, because this project depends on [CustomLocalization4EditorExtension], you need to
add `"com.github.sayamame-beans.vrcphysbone-relocator": "https://github.com/anatawa12/CustomLocalization4EditorExtension.git"`
into `dependencies` block of `manifest.json` of your project.

[CustomLocalization4EditorExtension]: https://openupm.com/packages/com.anatawa12.custom-localization-for-editor-extension/
