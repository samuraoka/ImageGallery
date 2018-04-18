"use strict";

/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

// Introduction to using Gulp in ASP.NET Core
// https://docs.microsoft.com/en-us/aspnet/core/client-side/using-gulp
// Sass
// https://docs.microsoft.com/en-us/aspnet/core/client-side/less-sass-fa#sass
// gulp
// https://www.npmjs.com/package/gulp
var gulp = require('gulp'),
    sass = require('gulp-sass'),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    rename = require("gulp-rename");

gulp.task('scss:css', function () {
    var sourceFiles = [
        './client/css/*.scss'
    ];
    return gulp.src(sourceFiles)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./client/css/dist'));
});

gulp.task('min:css', ['scss:css'], function () {
    return gulp.src('./client/css/dist/*.css')
        .pipe(cssmin())
        .pipe(rename({suffix:'.min'}))
        .pipe(gulp.dest('./client/css/dist/'));
});

gulp.task('copy:js', function () {
    var sourceFiles = [
        '../node_modules/bootstrap/dist/js/*',
        '../node_modules/jquery/dist/*',
        'client/js/*'
    ];
    return gulp.src(sourceFiles)
        .pipe(gulp.dest('wwwroot/js'));
});

gulp.task('copy:css', ['scss:css', 'min:css'], function () {
    var sourceFiles = [
        '../node_modules/bootstrap/dist/css/*',
        'client/css/dist/*'
    ];
    return gulp.src(sourceFiles)
        .pipe(gulp.dest('wwwroot/css'));
});

// The default task (called when you run `gulp` from cli)
gulp.task('default', ['copy:js', 'copy:css'], function () {});
