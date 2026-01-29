if (isActive === true) {
  document.getElementById('darkRadio1').checked = true;
} else {
  document.getElementById('darkRadio2').checked = true;
}

$(document).ready(function () {

  function updateCarouselControls() {
    if ($(".carousel-item").length > 1) {
      $(".carousel-control-prev, .carousel-control-next").show();
    } else {
      $(".carousel-control-prev, .carousel-control-next").hide();
    }
  }

  $(document).on('click', '.close-btn', function () {
    const carouselItem = $(this).closest('.carousel-item');
    carouselItem.remove();

    if ($(".carousel-item").length === 0) {
      $('input[type="file"]').val('');
    } else if (!$(".carousel-item.active").length) {
      $(".carousel-item").first().addClass("active");
    }

    updateCarouselControls();
  });

  $('input[type="file"]').on('change', function () {
    const files = this.files;
    const carouselInner = $(".carousel-inner");

    if (files.length > 0) {
      Array.from(files).forEach((file, index) => {
        if (file.type.startsWith('image/')) {
          const fileReader = new FileReader();
          fileReader.onload = function (e) {
            const carouselItem = $(`
            <div class="carousel-item">
              <img class="d-block w-100" src="${e.target.result}" alt="Önizleme Görseli">
              <button type="button" class="close-btn">&times;</button>
            </div>
          `);

            // İlk görsele 'active' sınıfını ekle
            if (!carouselInner.find('.carousel-item').length) {
              carouselItem.addClass('active');
            }
            carouselInner.append(carouselItem);

            updateCarouselControls();
          };
          fileReader.readAsDataURL(file);
        }
      });
    }
  });

  updateCarouselControls();
});

    const startRecordButton = document.getElementById('startRecord');
    const deleteButton = document.getElementById('deleteRecord');
    const playPauseButton = document.getElementById('playPause');
    const muteButton = document.getElementById('mute');
    const audio = document.getElementById('audioPlayback');
    const progressBar = document.getElementById('progress');
    let mediaRecorder;
    let audioChunks = [];
    let isRecording = false;
    let isPlaying = false;
    let audioBlob;
    let currentAudioURL = null;



if (audioUrl && audioUrl.trim() !== "") {
  audio.src = audioUrl; 

} else {
  audio.style.display = "none";   
}

    startRecordButton.onclick = (event) => {
      event.preventDefault();
      if (isRecording) {
        mediaRecorder.stop();
        startRecordButton.classList.remove('flash');
        isRecording = false;
      } else {
        startRecording();
        startRecordButton.classList.add('flash');
        isRecording = true;
      }
    };

    function startRecording() {
      navigator.mediaDevices.getUserMedia({ audio: true })
        .then(stream => {
          mediaRecorder = new MediaRecorder(stream);

          mediaRecorder.ondataavailable = event => {
            audioChunks.push(event.data);
          };

          mediaRecorder.onstop = () => {
            audioBlob = new Blob(audioChunks, { type: 'audio/wav' });
            console.log('Audio Blob:', audioBlob);
            const audioURL = URL.createObjectURL(audioBlob);
            audio.src = audioURL;
            audioChunks = [];
          };

          mediaRecorder.start();
        })
        .catch(error => {
          console.error("Ses kaydında bir hata oluştu:", error);
        });
    }

    playPauseButton.onclick = (event) => {
      event.preventDefault();

      if (!audio.src || audio.src === window.location.href) {
        console.warn("Ses çalma uyarısı: Geçerli ses kaynağı yok.");
        return;
      }

      if (isPlaying) {
        audio.pause();
        playPauseButton.innerHTML = "<i class='bi bi-play-circle-fill'></i>";
        isPlaying = false;
      } else {
        audio.play().catch(error => {
          console.error("Ses çalma hatası:", error);
        });
        playPauseButton.innerHTML = "<i class='bi bi-pause-circle-fill'></i>";
        isPlaying = true;
      }
    };

    audio.onplay = () => {
      isPlaying = true;
      setInterval(() => {
        const progress = (audio.currentTime / audio.duration) * 100;
        progressBar.value = progress;
      }, 1000);
    };

    audio.onpause = () => {
      isPlaying = false;
    };

    document.getElementById('attachFile').addEventListener('click', function (event) {
      event.preventDefault(); 
      document.getElementById('audioFileUpload').click(); 
    });


    deleteButton.onclick = (event) => {
      event.preventDefault();

      if (currentAudioURL) {
        URL.revokeObjectURL(currentAudioURL);
        currentAudioURL = null;
      }

      audio.pause();
      audio.src = "";
      audio.currentTime = 0;
      progressBar.value = 0;
      audioBlob = null;
      audioChunks = [];
      startRecordButton.classList.remove('flash');
      isRecording = false;

      document.getElementById('audioFileUpload').value = "";
    };

    document.getElementById('audioFileUpload').addEventListener('change', function (event) {
      event.preventDefault();

      const audioFile = event.target.files[0];
      if (audioFile) {
        const fileType = audioFile.type;

        if (fileType.startsWith('audio/')) {
    
          if (currentAudioURL) {
            URL.revokeObjectURL(currentAudioURL);
          }

          currentAudioURL = URL.createObjectURL(audioFile);

          audio.pause();
          audio.src = currentAudioURL;
          audio.currentTime = 0;
          progressBar.value = 0;

          isPlaying = false;
          playPauseButton.innerHTML = "<i class='bi bi-play-circle-fill'></i>";
        } else {
 
          alert("Lütfen bir ses dosyası seçin.");
        }
      }
    });


