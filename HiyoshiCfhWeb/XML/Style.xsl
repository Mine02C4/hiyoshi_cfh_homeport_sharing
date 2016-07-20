<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:x="http://hiyoshicfh.pyonpyon.tokyo/Quests.xsd">
  <xsl:output indent="yes" method="html" encoding="utf-8"/>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="x:Quests">
    <html>
      <head>
        <title>任務一覧</title>
        <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet"/>
      </head>
      <body>
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>カテゴリ</th>
              <th>タイプ</th>
              <th>任務名</th>
              <th>内容</th>
              <th>燃料</th>
              <th>弾薬</th>
              <th>鋼材</th>
              <th>ボーキサイト</th>
              <th>高速建造材</th>
              <th>高速修復材</th>
              <th>開発資材</th>
              <th>改修資材</th>
              <th>その他の報酬</th>
              <th>依存関係</th>
            </tr>
          </thead>
          <tbody>
            <xsl:apply-templates/>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="x:Quest">
    <tr>
      <td>
        <xsl:value-of select="@Id"/>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@Category='composition'">編成</xsl:when>
          <xsl:when test="@Category='sortie'">出撃</xsl:when>
          <xsl:when test="@Category='practice'">演習</xsl:when>
          <xsl:when test="@Category='expeditions'">遠征</xsl:when>
          <xsl:when test="@Category='supply'">補給/入渠</xsl:when>
          <xsl:when test="@Category='building'">工廠</xsl:when>
          <xsl:when test="@Category='remodelling'">改装</xsl:when>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@Type='onetime'">単発</xsl:when>
          <xsl:when test="@Type='daily'">デイリー</xsl:when>
          <xsl:when test="@Type='weekly'">ウィークリー</xsl:when>
          <xsl:when test="@Type='monthly'">マンスリー</xsl:when>
          <xsl:when test="@Type='quarterly'">クォータリー</xsl:when>
          <xsl:when test="@Type='other'">その他</xsl:when>
        </xsl:choose>
      </td>
      <xsl:apply-templates/>
    </tr>
  </xsl:template>
  <xsl:template match="x:Bonus">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="x:Dependency">
    <td>
      <ul>
        <xsl:apply-templates/>
      </ul>
    </td>
  </xsl:template>
  <xsl:template match="x:Achieve">
    <li>
      <xsl:value-of select="@Id"/>
    </li>
  </xsl:template>
  <xsl:template match="x:Quest//*" priority="-1">
    <td>
      <xsl:value-of select="."/>
    </td>
  </xsl:template>
</xsl:stylesheet>