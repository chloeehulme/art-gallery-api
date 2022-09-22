CREATE DATABASE ARTGALLERY;

CREATE TABLE public.state (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(50) NOT NULL,
    language_groups INT NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE public.artist (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    age INT NULL,
    state_id INT NOT NULL,
    language_group VARCHAR(100) NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id),
);

CREATE TABLE public.artefact (
    id INT NOT NULL GENERATED ALWAYS AS IDENTITY,
    artist_id INT NOT NULL,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(800) NULL,
    medium VARCHAR(50) NOT NULL,
    year INT NOT NULL,
    height_cm DECIMAL NULL,
    width_cm DECIMAL NULL,
    img_url VARCHAR(100) NOT NULL,
    created_date TIMESTAMP NOT NULL,
    modified_date TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);