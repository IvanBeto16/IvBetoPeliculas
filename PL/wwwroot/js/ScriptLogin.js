﻿const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');

//signUpButton.addEventListener('click', () => {
//	container.classList.add("right-panel-active");
//});

//signInButton.addEventListener('click', () => {
//	container.classList.remove("right-panel-active");
//});

function swipeSignUp() {
    const signUpButton = document.getElementById('signUp');
    signUpButton.addEventListener('click', () => {
        container.classList.add("right-panel-active")
    });
}
