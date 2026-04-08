// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
  const alerts = document.querySelectorAll(".app-toast-alert");

  alerts.forEach((alert) => {
    window.setTimeout(() => {
      const closeButton = alert.querySelector(".btn-close");
      if (closeButton) {
        closeButton.click();
      }
    }, 4500);
  });
});
