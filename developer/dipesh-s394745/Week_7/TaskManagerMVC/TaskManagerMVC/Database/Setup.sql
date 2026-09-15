CREATE TABLE IF NOT EXISTS Tasks
(
    Id            SERIAL PRIMARY KEY,
    Title         VARCHAR(200)  NOT NULL,
    Description   VARCHAR(1000) NULL,
    IsCompleted   BOOLEAN       NOT NULL DEFAULT FALSE,
    CreatedAt     TIMESTAMP     NOT NULL DEFAULT NOW()
);
