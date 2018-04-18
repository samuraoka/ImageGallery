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
    gulpCopy = require('gulp-copy');

gulp.task('copy:js', function () {
    var sourceFiles = [
        '../node_modules/bootstrap/dist/js/*',
        '../node_modules/jquery/dist/*',
        'client/js/*'
    ];
    return gulp.src(sourceFiles)
        .pipe(gulp.dest('wwwroot/js'));
});

// The default task (called when you run `gulp` from cli)
gulp.task('default', ['copy:js'], function () {
    // place code for your default task here
});
