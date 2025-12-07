// Contact Form Handler

$(document).ready(function () {
    // Form submit
    $('#contactForm').on('submit', function (e) {
        e.preventDefault();

        // Lấy dữ liệu form
        var formData = {
            fullName: $('#fullName').val().trim(),
            email: $('#email').val().trim(),
            phone: $('#phone').val().trim(),
            subject: $('#subject').val(),
            message: $('#message').val().trim()
        };

        // Kiểm tra đơn giản
        if (!formData.fullName || !formData.email || !formData.phone || !formData.subject || !formData.message) {
            showAlert('Vui lòng điền đầy đủ thông tin!', 'error');
            return;
        }

        // Kiểm tra email cơ bản
        if (!formData.email.includes('@')) {
            showAlert('Email không hợp lệ!', 'error');
            $('#email').focus();
            return;
        }

        // Hiển thị thông báo đang gửi
        showAlert('Đang gửi tin nhắn...', 'info');

        // Disable button để tránh spam
        var $submitBtn = $('.btn-send');
        var originalText = $submitBtn.html();
        $submitBtn.prop('disabled', true).html('<i class="fa-solid fa-spinner fa-spin me-2"></i>Đang gửi...');

        // Gửi dữ liệu đến server
        $.ajax({
            url: '/Contact/SendMessage',
            type: 'POST',
            data: formData,
            success: function (response) {
                // Xóa thông báo "Đang gửi..."
                $('.custom-alert').remove();

                if (response.success) {
                    showAlert(response.message, 'success');
                    // Reset form sau khi gửi thành công
                    $('#contactForm')[0].reset();
                } else {
                    showAlert(response.message, 'error');
                }
            },
            error: function (xhr, status, error) {
                // Xóa thông báo "Đang gửi..."
                $('.custom-alert').remove();

                console.error('Error:', error);
                showAlert('Có lỗi xảy ra khi gửi tin nhắn. Vui lòng thử lại!', 'error');
            },
            complete: function () {
                // Enable lại button
                $submitBtn.prop('disabled', false).html(originalText);
            }
        });
    });

    // Function hiển thị thông báo đẹp
    function showAlert(message, type) {
        var icon = type === 'success' ? '✅' : (type === 'info' ? '⏳' : '❌');
        var bgColor = type === 'success' ? '#4CAF50' : (type === 'info' ? '#2196F3' : '#f44336');

        // Xóa alert cũ nếu có
        $('.custom-alert').remove();

        // Tạo alert element
        var alertHtml = `
            <div class="custom-alert" style="
                position: fixed;
                top: 100px;
                right: 20px;
                background: ${bgColor};
                color: white;
                padding: 20px 30px;
                border-radius: 10px;
                box-shadow: 0 5px 20px rgba(0,0,0,0.3);
                z-index: 9999;
                animation: slideIn 0.3s ease;
                max-width: 400px;
            ">
                <div style="display: flex; align-items: center;">
                    <span style="font-size: 24px; margin-right: 15px;">${icon}</span>
                    <span style="font-size: 16px;">${message}</span>
                </div>
            </div>
        `;

        // CSS animation
        if ($('#alertAnimation').length === 0) {
            $('head').append(`
                <style id="alertAnimation">
                    @keyframes slideIn {
                        from {
                            transform: translateX(400px);
                            opacity: 0;
                        }
                        to {
                            transform: translateX(0);
                            opacity: 1;
                        }
                    }
                    @keyframes slideOut {
                        from {
                            transform: translateX(0);
                            opacity: 1;
                        }
                        to {
                            transform: translateX(400px);
                            opacity: 0;
                        }
                    }
                </style>
            `);
        }

        // Thêm alert vào body
        var $alert = $(alertHtml);
        $('body').append($alert);

        // Tự động ẩn sau 5 giây (trừ khi là thông báo info)
        if (type !== 'info') {
            setTimeout(function () {
                $alert.css('animation', 'slideOut 0.3s ease');
                setTimeout(function () {
                    $alert.remove();
                }, 300);
            }, 5000);
        }
    }

    // Smooth scroll animation cho contact info items
    $('.contact-info-item').each(function (index) {
        $(this).css({
            'opacity': '0',
            'transform': 'translateY(20px)'
        });

        setTimeout(() => {
            $(this).css({
                'opacity': '1',
                'transform': 'translateY(0)',
                'transition': 'all 0.5s ease'
            });
        }, index * 100);
    });
});