-- Script Date: 2019/11/27 16:20  - ErikEJ.SqlCeScripting version 3.5.2.81
CREATE TABLE [IMAGE_SOURCES] (
  [ZIP_SETTINGS_ID] INTEGER NOT NULL
, [IMAGE_SOURCE_PATH] TEXT NOT NULL
, [SOURCE_KIND] INTEGER
, CONSTRAINT [PK_ImageSources] PRIMARY KEY ([ZIP_SETTINGS_ID], IMAGE_SOURCE_PATH)
);