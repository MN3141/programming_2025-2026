import PyQt6.QtCore as qcore


class MLWorker(qcore.QObject):
    resultReady = qcore.pyqtSignal(int)

    def __init__(self):
        super().__init__()
        self._epoch_count = 0

    def stepEpochSlot(self):
        self._epoch_count += 1
        self.resultReady.emit(self._epoch_count)
