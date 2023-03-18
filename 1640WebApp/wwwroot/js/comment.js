// Lấy thẻ HTML của nút "Comment"
const commentBtn = document.querySelector('.action');

// Thêm sự kiện click vào nút "Comment"
commentBtn.addEventListener('click', function () {
    // Hiển thị hộp thoại nhập bình luận
    // (ví dụ: sử dụng Bootstrap Modal)
    // ...

    // Sau khi người dùng gửi bình luận, gửi dữ liệu lên server bằng Ajax
    // và lấy 3 bình luận gần nhất trả về từ server
    const xhr = new XMLHttpRequest();
    xhr.open('POST', '/Idea/GetRecentComments');
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function () {
        if (xhr.status === 200) {
            const comments = JSON.parse(xhr.responseText);
            // Hiển thị 3 bình luận gần nhất trên trang
            // (ví dụ: sử dụng thẻ HTML để tạo các phần tử DOM)
            // ...
        } else {
            console.error('Error:', xhr.statusText);
        }
    };
    xhr.onerror = function () {
        console.error('Error:', xhr.statusText);
    };
    xhr.send(JSON.stringify({ ideaId: ideaId }));
});
