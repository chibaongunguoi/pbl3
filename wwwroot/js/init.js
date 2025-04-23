console.log("init.js loaded");

function initRatingPopupStars(semesterId, stars) {
  const modal = document.querySelector(`.modal[semesterId='${semesterId}']`);
  const starWrappers = modal.querySelectorAll(".rating-star-wrapper");
  console.log(starWrappers);
  const closeButton = modal.querySelector('.close-popup');
  const ratingInput = document.getElementById('rating');
  let selectedRating = stars
  starWrappers.forEach((wrapper, index) => {
    // Sự kiện mouseover
    wrapper.addEventListener('mouseover', () => {
      highlightStars(index + 1);
    });

    // Sự kiện click
    wrapper.addEventListener('click', () => {
      selectedRating = index + 1;
      ratingInput.value = selectedRating; // Cập nhật giá trị input ẩn
    });
  });
  closeButton.addEventListener('click', () => {
    selectedRating = stars
    highlightStars(selectedRating);
    ratingInput.value = selectedRating;
  });
  // Sự kiện mouseout cho container chính
  function highlightStars(count) {
    starWrappers.forEach((wrapper, idx) => {
      const fillElement = wrapper.querySelector('.rating-star-fill');
      fillElement.style.width = idx < count ? '100%' : '0%';
    });
  }
  modal.querySelector('.card-product-rating-star').addEventListener('mouseout', () => {
    if (selectedRating > 0) {
      highlightStars(selectedRating);
    } else {
      // Reset về trạng thái ban đầu nếu không có rating nào được chọn
      starWrappers.forEach(wrapper => {
        const fillElement = wrapper.querySelector('.rating-star-fill');
        fillElement.style.width = '0%'; // Hoặc giá trị mặc định của bạn
      });
    }
  });
  highlightStars(selectedRating);
}