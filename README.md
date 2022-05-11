# VRCPhysBone-Relocator
Relocate the PhysBone and PhysBoneCollider Components to the object set in the "Root Transform".\
This is useful when the object corresponding to the PhysBone is hard to find (e.g. VRoid Avatar).

PhysBoneコンポーネント類を\"Root Transform\"に設定されているゲームオブジェクトに再配置します。\
1つのゲームオブジェクトに大量のPhysBoneコンポーネント類が含まれていて、対応するゲームオブジェクトが分かりづらい場合などに役に立ちます。(変換直後のVRoid製アバターなど)

## How to install/導入方法
[EN]
1. Open your unity project.
2. Open the Package Manager from Window → Package Manager at the top of the Editor window.
3. Click `+` at the upper left of the Package Manager window , then select `Add package from git URL...`.
4. Enter `https://github.com/Sayamame-beans/VRCPhysBone-Relocator.git` , then press "Add".

[JP]
1. Unityプロジェクトを開きます。
2. エディタ上部のWindows → Package Managerから、Package Managerを開きます。
3. Package Managerウィンドウの左上の`+`をクリックし、`Add package from git URL...`を選択します。
4. `https://github.com/Sayamame-beans/VRCPhysBone-Relocator.git`と入力し、"Add"を押します。

## Usage/使い方
1. Open the window from Tools → PB Relocator at the top of the Editor window.
2. Set the target GameObject, then press "Relocate!"

You can Undo/Redo this operation.

1. エディタ上部のTools → PB Relocatorから、ウィンドウを開きます。
2. 対象のゲームオブジェクトをセットし、"Relocate!"ボタンを押します。

この操作は元に戻したり、やり直したりする事が出来ます。

---

This repository is WIP.\
このリポジトリは制作中です。
