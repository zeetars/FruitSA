window.DownloadExcelFile = (fileName, fileData, contentType) => {
    const blob = new Blob([new Uint8Array(fileData)], { type: contentType });

    if (navigator.msSaveBlob) { // IE10+
        navigator.msSaveBlob(blob, fileName);
    } else {
        const link = document.createElement('a');
        if (link.download !== undefined) {
            const url = URL.createObjectURL(blob);
            link.setAttribute('href', url);
            link.setAttribute('download', fileName);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
};