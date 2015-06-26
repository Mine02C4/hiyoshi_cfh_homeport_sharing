# 日吉連合艦隊司令部(Combined Fleet Headquarters of Hiyoshi)

司令官さん、いつも提督業お疲れ様なのです。優秀な司令官は着任してない時でも情報整理や戦略立案を欠かさないようです。司令官さんもそうですか？
でも、着任していないと艦隊情報を確認できないですし、不便だと思うのです。そこで司令官さんを手助けする方法を思いついたのです！
ウェブ技術を活用して、艦隊情報を外から確認できるようにするのです！そうすれば、どこでも、いつでも、私達の未来を切り開けるのです！
あと、親しい他の司令官と艦隊情報を共有して相談もしやすくなるのです！

…ど、どうでしょう…この方法、問題…ないですか？

## 基本構成

[「提督業も忙しい！」](http://grabacr.net/kancolleviewer)にプラグインを追加して、艦隊情報をウェブ経由でデータベースに格納し、ウェブなどの様々な方法で閲覧します。
サーバー側のシステムは[Microsoft Azure](http://azure.microsoft.com/ja-jp/)で実行することを想定しています。

### 艦隊情報更新

+----------------------------------------+  OData   +---------------+  ADO.NET   +------------+
| 「提督業も忙しい！」(HiyoshiCfhClient) | -------> | HiyoshiCfhWeb | ---------> | SQL Server |
+----------------------------------------+          +---------------+            +------------+

## 開発環境の準備

* Visual Studio Ultimate, Community, Professional, Premium 2013

上記ツールでソリューションを開き、NuGetで依存ライブラリをダウンロードしてください。

## 使用言語

* C♯
* ウェブ周りではJavaScriptも使います

## 使用技術(ライブラリやフレームワーク等)

### ウェブフロントエンド

* ASP.NET MVC 5 (ウェブアプリケーションフレームワーク)
** Razor (ビューエンジン)
* ASP.NET Identity (認証)
** OAuth 2.0, OpenID Connect (Google経由の認証に使用。基本的にはライブラリに丸投げ)

### データベース

* ADO.NET Entity Framework (ORM)
** Code First Migrationを使用している。

### ウェブAPI

* ASP.NET Web API 2.2
* OData v4

### 「提督業も忙しい！」プラグイン

* Windows Presentation Foundation (ユーザーインターフェースサブシステム)
* Livet (MVVMインフラストラクチャ)
* OData v4

## 予定

* Androidクライアントの作成

