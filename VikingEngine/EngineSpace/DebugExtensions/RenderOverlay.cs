using System;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.DebugExtensions
{
    public class RenderOverlay
    {
        public static RenderOverlay Instance = new RenderOverlay();
        public static readonly string LayoutSeparator = MemoryOverlay.LayoutSeparator;

        public bool IsEnabled = true;

        // Rolling metrics for current frame accumulation
        private float _minRenderTimeMs = float.MaxValue;
        private float _maxRenderTimeMs = 0f;
        private float _totalRenderTimeMs = 0f;
        private uint _sampleCount = 0;

        private float _totalPrepBatchesTimeMs = 0f;
        private float _maxPrepBatchesTimeMs = 0f;

        private float _totalDrawDepthTimeMs = 0f;
        private float _maxDrawDepthTimeMs = 0f;

        private float _totalDrawLitTimeMs = 0f;
        private float _maxDrawLitTimeMs = 0f;

        private int _totalStandardDrawCalls = 0;
        private int _totalInstancedDrawCalls = 0;
        private int _totalRenderedInstances = 0;
        private int _totalInstancedBatches = 0;
        private int _totalFrameSlices = 0;
        private long _totalUploadedBytes = 0;

        // Simulation update rolling metrics
        private float _minUpdateTimeMs = float.MaxValue;
        private float _maxUpdateTimeMs = 0f;
        private float _totalUpdateTimeMs = 0f;
        private uint _updateSampleCount = 0;

        // GPU Present rolling metrics
        private float _totalPresentTimeMs = 0f;
        private float _maxPresentTimeMs = 0f;
        private uint _presentSampleCount = 0;

        // Updates-per-frame rolling metrics
        private int _totalUpdatesPerFrame = 0;
        private int _maxUpdatesPerFrame = 0;
        private uint _updatesPerFrameSampleCount = 0;

        // Simulation subsystem rolling metrics (Phase 4)
        private float _totalCitiesMs = 0f;
        private float _maxCitiesMs = 0f;
        private float _totalFactionsMs = 0f;
        private float _maxFactionsMs = 0f;
        private float _totalFactionOneSecMs = 0f;
        private float _maxFactionOneSecMs = 0f;
        private float _totalMapMs = 0f;
        private float _maxMapMs = 0f;
        private float _totalUserInputMs = 0f;
        private float _maxUserInputMs = 0f;
        private float _totalParticlesMs = 0f;
        private float _maxParticlesMs = 0f;
        private uint _simSubsystemSampleCount = 0;

        // Engine loop rolling metrics (Phase 4.1)
        private float _totalEngineCalcDeltaMs = 0f;
        private float _maxEngineCalcDeltaMs = 0f;
        private float _totalEngineUpdateListMs = 0f;
        private float _maxEngineUpdateListMs = 0f;
        private float _totalEngineSyncQueMs = 0f;
        private float _maxEngineSyncQueMs = 0f;
        private float _totalEngineGameStateMs = 0f;
        private float _maxEngineGameStateMs = 0f;
        private float _totalEngineInputSoundMs = 0f;
        private float _maxEngineInputSoundMs = 0f;
        private float _totalEngineLazyUpdateMs = 0f;
        private float _maxEngineLazyUpdateMs = 0f;
        private uint _engineSubsystemSampleCount = 0;

        // Aggregated 1-second results
        public int FPS { get; private set; } = 0;
        public float MinRenderTimeMs { get; private set; } = 0f;
        public float MaxRenderTimeMs { get; private set; } = 0f;
        public float AvgRenderTimeMs { get; private set; } = 0f;
        public double PeakRenderTimeMs { get; private set; } = 0;

        public float AvgPrepBatchesTimeMs { get; private set; } = 0f;
        public float PeakPrepBatchesTimeMs { get; private set; } = 0f;

        public float AvgDrawDepthTimeMs { get; private set; } = 0f;
        public float PeakDrawDepthTimeMs { get; private set; } = 0f;

        public float AvgDrawLitTimeMs { get; private set; } = 0f;
        public float PeakDrawLitTimeMs { get; private set; } = 0f;

        public float AvgUpdateTimeMs { get; private set; } = 0f;
        public float MinUpdateTimeMs { get; private set; } = 0f;
        public float MaxUpdateTimeMs { get; private set; } = 0f;
        public double PeakUpdateTimeMs { get; private set; } = 0;

        public float AvgStandardDrawCallsPerFrame { get; private set; } = 0f;
        public float AvgInstancedDrawCallsPerFrame { get; private set; } = 0f;
        public float AvgTotalDrawCallsPerFrame { get; private set; } = 0f;
        public float AvgRenderedInstancesPerFrame { get; private set; } = 0f;
        public float AvgInstancedBatchesPerFrame { get; private set; } = 0f;
        public float AvgFrameSlicesPerFrame { get; private set; } = 0f;
        public float AvgUploadedKBPerFrame { get; private set; } = 0f;

        public float AvgPresentTimeMs { get; private set; } = 0f;
        public float MaxPresentTimeMs { get; private set; } = 0f;

        public float AvgUpdatesPerFrame { get; private set; } = 0f;
        public int PeakUpdatesPerFrame { get; private set; } = 0;

        // Simulation subsystem 1-second results (Phase 4)
        public float AvgCitiesMs { get; private set; } = 0f;
        public float PeakCitiesMs { get; private set; } = 0f;
        public float AvgFactionsMs { get; private set; } = 0f;
        public float PeakFactionsMs { get; private set; } = 0f;
        public float AvgFactionOneSecMs { get; private set; } = 0f;
        public float PeakFactionOneSecMs { get; private set; } = 0f;
        public float AvgMapMs { get; private set; } = 0f;
        public float PeakMapMs { get; private set; } = 0f;
        public float AvgUserInputMs { get; private set; } = 0f;
        public float PeakUserInputMs { get; private set; } = 0f;
        public float AvgParticlesMs { get; private set; } = 0f;
        public float PeakParticlesMs { get; private set; } = 0f;

        // Engine loop 1-second results (Phase 4.1)
        public float AvgEngineCalcDeltaMs { get; private set; } = 0f;
        public float PeakEngineCalcDeltaMs { get; private set; } = 0f;
        public float AvgEngineUpdateListMs { get; private set; } = 0f;
        public float PeakEngineUpdateListMs { get; private set; } = 0f;
        public float AvgEngineSyncQueMs { get; private set; } = 0f;
        public float PeakEngineSyncQueMs { get; private set; } = 0f;
        public float AvgEngineGameStateMs { get; private set; } = 0f;
        public float PeakEngineGameStateMs { get; private set; } = 0f;
        public float AvgEngineInputSoundMs { get; private set; } = 0f;
        public float PeakEngineInputSoundMs { get; private set; } = 0f;
        public float AvgEngineLazyUpdateMs { get; private set; } = 0f;
        public float PeakEngineLazyUpdateMs { get; private set; } = 0f;

        public string FormattedText { get; private set; } = string.Empty;

        public RenderOverlay()
        {
        }

        public void RecordUpdate(float updateTimeMs)
        {
            if (updateTimeMs < _minUpdateTimeMs)
            {
                _minUpdateTimeMs = updateTimeMs;
            }
            if (updateTimeMs > _maxUpdateTimeMs)
            {
                _maxUpdateTimeMs = updateTimeMs;
            }
            _totalUpdateTimeMs += updateTimeMs;
            _updateSampleCount++;
        }

        public void RecordPresent(float presentTimeMs)
        {
            if (presentTimeMs > _maxPresentTimeMs)
            {
                _maxPresentTimeMs = presentTimeMs;
            }
            _totalPresentTimeMs += presentTimeMs;
            _presentSampleCount++;
        }

        public void RecordUpdatesPerFrame(int updatesThisFrame)
        {
            if (updatesThisFrame > _maxUpdatesPerFrame)
            {
                _maxUpdatesPerFrame = updatesThisFrame;
            }
            _totalUpdatesPerFrame += updatesThisFrame;
            _updatesPerFrameSampleCount++;
        }

        public void RecordSimSubsystems(
            float citiesMs,
            float factionsMs,
            float factionOneSecMs,
            float mapMs,
            float userInputMs,
            float particlesMs)
        {
            if (citiesMs > _maxCitiesMs)
            {
                _maxCitiesMs = citiesMs;
            }
            _totalCitiesMs += citiesMs;

            if (factionsMs > _maxFactionsMs)
            {
                _maxFactionsMs = factionsMs;
            }
            _totalFactionsMs += factionsMs;

            if (factionOneSecMs > _maxFactionOneSecMs)
            {
                _maxFactionOneSecMs = factionOneSecMs;
            }
            _totalFactionOneSecMs += factionOneSecMs;

            if (mapMs > _maxMapMs)
            {
                _maxMapMs = mapMs;
            }
            _totalMapMs += mapMs;

            if (userInputMs > _maxUserInputMs)
            {
                _maxUserInputMs = userInputMs;
            }
            _totalUserInputMs += userInputMs;

            if (particlesMs > _maxParticlesMs)
            {
                _maxParticlesMs = particlesMs;
            }
            _totalParticlesMs += particlesMs;

            _simSubsystemSampleCount++;
        }

        public void RecordEngineSubsystems(
            float calcDeltaMs,
            float updateListMs,
            float syncQueMs,
            float gameStateMs,
            float inputSoundMs,
            float lazyUpdateMs)
        {
            if (calcDeltaMs > _maxEngineCalcDeltaMs)
            {
                _maxEngineCalcDeltaMs = calcDeltaMs;
            }
            _totalEngineCalcDeltaMs += calcDeltaMs;

            if (updateListMs > _maxEngineUpdateListMs)
            {
                _maxEngineUpdateListMs = updateListMs;
            }
            _totalEngineUpdateListMs += updateListMs;

            if (syncQueMs > _maxEngineSyncQueMs)
            {
                _maxEngineSyncQueMs = syncQueMs;
            }
            _totalEngineSyncQueMs += syncQueMs;

            if (gameStateMs > _maxEngineGameStateMs)
            {
                _maxEngineGameStateMs = gameStateMs;
            }
            _totalEngineGameStateMs += gameStateMs;

            if (inputSoundMs > _maxEngineInputSoundMs)
            {
                _maxEngineInputSoundMs = inputSoundMs;
            }
            _totalEngineInputSoundMs += inputSoundMs;

            if (lazyUpdateMs > _maxEngineLazyUpdateMs)
            {
                _maxEngineLazyUpdateMs = lazyUpdateMs;
            }
            _totalEngineLazyUpdateMs += lazyUpdateMs;

            _engineSubsystemSampleCount++;
        }

        public void RecordFrame(
            float renderTimeMs,
            float prepBatchesTimeMs = 0f,
            float drawDepthTimeMs = 0f,
            float drawLitTimeMs = 0f,
            int standardDrawCalls = 0,
            int instancedDrawCalls = 0,
            int renderedInstances = 0,
            int batchCount = 0,
            int frameSliceCount = 0,
            long uploadedBytes = 0)
        {
            if (renderTimeMs < _minRenderTimeMs)
            {
                _minRenderTimeMs = renderTimeMs;
            }
            if (renderTimeMs > _maxRenderTimeMs)
            {
                _maxRenderTimeMs = renderTimeMs;
            }
            _totalRenderTimeMs += renderTimeMs;

            if (prepBatchesTimeMs > _maxPrepBatchesTimeMs)
            {
                _maxPrepBatchesTimeMs = prepBatchesTimeMs;
            }
            _totalPrepBatchesTimeMs += prepBatchesTimeMs;

            if (drawDepthTimeMs > _maxDrawDepthTimeMs)
            {
                _maxDrawDepthTimeMs = drawDepthTimeMs;
            }
            _totalDrawDepthTimeMs += drawDepthTimeMs;

            if (drawLitTimeMs > _maxDrawLitTimeMs)
            {
                _maxDrawLitTimeMs = drawLitTimeMs;
            }
            _totalDrawLitTimeMs += drawLitTimeMs;

            _totalStandardDrawCalls += standardDrawCalls;
            _totalInstancedDrawCalls += instancedDrawCalls;
            _totalRenderedInstances += renderedInstances;
            _totalInstancedBatches += batchCount;
            _totalFrameSlices += frameSliceCount;
            _totalUploadedBytes += uploadedBytes;

            _sampleCount++;
        }

        public void UpdateOneSecond(int frameCount, double renderPeak, double updatePeak)
        {
            FPS = frameCount;
            PeakRenderTimeMs = renderPeak;
            PeakUpdateTimeMs = updatePeak;

            if (_sampleCount > 0)
            {
                MinRenderTimeMs = _minRenderTimeMs;
                MaxRenderTimeMs = _maxRenderTimeMs;
                AvgRenderTimeMs = _totalRenderTimeMs / _sampleCount;

                AvgPrepBatchesTimeMs = _totalPrepBatchesTimeMs / _sampleCount;
                PeakPrepBatchesTimeMs = _maxPrepBatchesTimeMs;

                AvgDrawDepthTimeMs = _totalDrawDepthTimeMs / _sampleCount;
                PeakDrawDepthTimeMs = _maxDrawDepthTimeMs;

                AvgDrawLitTimeMs = _totalDrawLitTimeMs / _sampleCount;
                PeakDrawLitTimeMs = _maxDrawLitTimeMs;

                AvgStandardDrawCallsPerFrame = (float)_totalStandardDrawCalls / _sampleCount;
                AvgInstancedDrawCallsPerFrame = (float)_totalInstancedDrawCalls / _sampleCount;
                AvgTotalDrawCallsPerFrame = AvgStandardDrawCallsPerFrame + AvgInstancedDrawCallsPerFrame;
                AvgRenderedInstancesPerFrame = (float)_totalRenderedInstances / _sampleCount;
                AvgInstancedBatchesPerFrame = (float)_totalInstancedBatches / _sampleCount;
                AvgFrameSlicesPerFrame = (float)_totalFrameSlices / _sampleCount;
                AvgUploadedKBPerFrame = (_totalUploadedBytes / 1024f) / _sampleCount;
            }
            else
            {
                MinRenderTimeMs = 0f;
                MaxRenderTimeMs = 0f;
                AvgRenderTimeMs = 0f;

                AvgPrepBatchesTimeMs = 0f;
                PeakPrepBatchesTimeMs = 0f;

                AvgDrawDepthTimeMs = 0f;
                PeakDrawDepthTimeMs = 0f;

                AvgDrawLitTimeMs = 0f;
                PeakDrawLitTimeMs = 0f;

                AvgStandardDrawCallsPerFrame = 0f;
                AvgInstancedDrawCallsPerFrame = 0f;
                AvgTotalDrawCallsPerFrame = 0f;
                AvgRenderedInstancesPerFrame = 0f;
                AvgInstancedBatchesPerFrame = 0f;
                AvgFrameSlicesPerFrame = 0f;
                AvgUploadedKBPerFrame = 0f;
            }

            if (_updateSampleCount > 0)
            {
                MinUpdateTimeMs = _minUpdateTimeMs;
                MaxUpdateTimeMs = _maxUpdateTimeMs;
                AvgUpdateTimeMs = _totalUpdateTimeMs / _updateSampleCount;
            }
            else
            {
                MinUpdateTimeMs = 0f;
                MaxUpdateTimeMs = 0f;
                AvgUpdateTimeMs = 0f;
            }

            if (_presentSampleCount > 0)
            {
                AvgPresentTimeMs = _totalPresentTimeMs / _presentSampleCount;
                MaxPresentTimeMs = _maxPresentTimeMs;
            }
            else
            {
                AvgPresentTimeMs = 0f;
                MaxPresentTimeMs = 0f;
            }

            if (_updatesPerFrameSampleCount > 0)
            {
                AvgUpdatesPerFrame = (float)_totalUpdatesPerFrame / _updatesPerFrameSampleCount;
                PeakUpdatesPerFrame = _maxUpdatesPerFrame;
            }
            else
            {
                AvgUpdatesPerFrame = 0f;
                PeakUpdatesPerFrame = 0;
            }

            // Reset rolling accumulators
            _minRenderTimeMs = float.MaxValue;
            _maxRenderTimeMs = 0f;
            _totalRenderTimeMs = 0f;
            _totalPrepBatchesTimeMs = 0f;
            _maxPrepBatchesTimeMs = 0f;
            _totalDrawDepthTimeMs = 0f;
            _maxDrawDepthTimeMs = 0f;
            _totalDrawLitTimeMs = 0f;
            _maxDrawLitTimeMs = 0f;

            _totalStandardDrawCalls = 0;
            _totalInstancedDrawCalls = 0;
            _totalRenderedInstances = 0;
            _totalInstancedBatches = 0;
            _totalFrameSlices = 0;
            _totalUploadedBytes = 0;
            _sampleCount = 0;

            _minUpdateTimeMs = float.MaxValue;
            _maxUpdateTimeMs = 0f;
            _totalUpdateTimeMs = 0f;
            _updateSampleCount = 0;

            _totalPresentTimeMs = 0f;
            _maxPresentTimeMs = 0f;
            _presentSampleCount = 0;

            _totalUpdatesPerFrame = 0;
            _maxUpdatesPerFrame = 0;
            _updatesPerFrameSampleCount = 0;

            // Aggregate simulation subsystem metrics (Phase 4)
            if (_simSubsystemSampleCount > 0)
            {
                AvgCitiesMs = _totalCitiesMs / _simSubsystemSampleCount;
                PeakCitiesMs = _maxCitiesMs;
                AvgFactionsMs = _totalFactionsMs / _simSubsystemSampleCount;
                PeakFactionsMs = _maxFactionsMs;
                AvgFactionOneSecMs = _totalFactionOneSecMs / _simSubsystemSampleCount;
                PeakFactionOneSecMs = _maxFactionOneSecMs;
                AvgMapMs = _totalMapMs / _simSubsystemSampleCount;
                PeakMapMs = _maxMapMs;
                AvgUserInputMs = _totalUserInputMs / _simSubsystemSampleCount;
                PeakUserInputMs = _maxUserInputMs;
                AvgParticlesMs = _totalParticlesMs / _simSubsystemSampleCount;
                PeakParticlesMs = _maxParticlesMs;
            }
            else
            {
                AvgCitiesMs = 0f;
                PeakCitiesMs = 0f;
                AvgFactionsMs = 0f;
                PeakFactionsMs = 0f;
                AvgFactionOneSecMs = 0f;
                PeakFactionOneSecMs = 0f;
                AvgMapMs = 0f;
                PeakMapMs = 0f;
                AvgUserInputMs = 0f;
                PeakUserInputMs = 0f;
                AvgParticlesMs = 0f;
                PeakParticlesMs = 0f;
            }

            // Reset simulation subsystem accumulators
            _totalCitiesMs = 0f;
            _maxCitiesMs = 0f;
            _totalFactionsMs = 0f;
            _maxFactionsMs = 0f;
            _totalFactionOneSecMs = 0f;
            _maxFactionOneSecMs = 0f;
            _totalMapMs = 0f;
            _maxMapMs = 0f;
            _totalUserInputMs = 0f;
            _maxUserInputMs = 0f;
            _totalParticlesMs = 0f;
            _maxParticlesMs = 0f;
            _simSubsystemSampleCount = 0;

            // Aggregate engine subsystem metrics (Phase 4.1)
            if (_engineSubsystemSampleCount > 0)
            {
                AvgEngineCalcDeltaMs = _totalEngineCalcDeltaMs / _engineSubsystemSampleCount;
                PeakEngineCalcDeltaMs = _maxEngineCalcDeltaMs;
                AvgEngineUpdateListMs = _totalEngineUpdateListMs / _engineSubsystemSampleCount;
                PeakEngineUpdateListMs = _maxEngineUpdateListMs;
                AvgEngineSyncQueMs = _totalEngineSyncQueMs / _engineSubsystemSampleCount;
                PeakEngineSyncQueMs = _maxEngineSyncQueMs;
                AvgEngineGameStateMs = _totalEngineGameStateMs / _engineSubsystemSampleCount;
                PeakEngineGameStateMs = _maxEngineGameStateMs;
                AvgEngineInputSoundMs = _totalEngineInputSoundMs / _engineSubsystemSampleCount;
                PeakEngineInputSoundMs = _maxEngineInputSoundMs;
                AvgEngineLazyUpdateMs = _totalEngineLazyUpdateMs / _engineSubsystemSampleCount;
                PeakEngineLazyUpdateMs = _maxEngineLazyUpdateMs;
            }
            else
            {
                AvgEngineCalcDeltaMs = 0f;
                PeakEngineCalcDeltaMs = 0f;
                AvgEngineUpdateListMs = 0f;
                PeakEngineUpdateListMs = 0f;
                AvgEngineSyncQueMs = 0f;
                PeakEngineSyncQueMs = 0f;
                AvgEngineGameStateMs = 0f;
                PeakEngineGameStateMs = 0f;
                AvgEngineInputSoundMs = 0f;
                PeakEngineInputSoundMs = 0f;
                AvgEngineLazyUpdateMs = 0f;
                PeakEngineLazyUpdateMs = 0f;
            }

            // Reset engine subsystem accumulators
            _totalEngineCalcDeltaMs = 0f;
            _maxEngineCalcDeltaMs = 0f;
            _totalEngineUpdateListMs = 0f;
            _maxEngineUpdateListMs = 0f;
            _totalEngineSyncQueMs = 0f;
            _maxEngineSyncQueMs = 0f;
            _totalEngineGameStateMs = 0f;
            _maxEngineGameStateMs = 0f;
            _totalEngineInputSoundMs = 0f;
            _maxEngineInputSoundMs = 0f;
            _totalEngineLazyUpdateMs = 0f;
            _maxEngineLazyUpdateMs = 0f;
            _engineSubsystemSampleCount = 0;

            float perFrameUpdateMs = AvgUpdateTimeMs * AvgUpdatesPerFrame;

            FormattedText = $"{FPS} FPS | Update: {AvgUpdateTimeMs:F1}ms x {AvgUpdatesPerFrame:F1} Upd/f = {perFrameUpdateMs:F1}ms | CPU Draw: {AvgRenderTimeMs:F1}ms (Prep: {AvgPrepBatchesTimeMs:F1}ms, Depth: {AvgDrawDepthTimeMs:F1}ms, Lit: {AvgDrawLitTimeMs:F1}ms) | Present: {AvgPresentTimeMs:F1}ms{LayoutSeparator}" +
                            $"Update Peak: {PeakUpdateTimeMs:F1}ms | VBO: {AvgUploadedKBPerFrame:F1} KB/f{LayoutSeparator}" +
                            $"DrawCalls: {AvgTotalDrawCallsPerFrame:F0} (Inst: {AvgInstancedDrawCallsPerFrame:F0}, Std: {AvgStandardDrawCallsPerFrame:F0}) | Units/Inst: {AvgRenderedInstancesPerFrame:F0} (Batches: {AvgInstancedBatchesPerFrame:F0}, Slices: {AvgFrameSlicesPerFrame:F0}){LayoutSeparator}" +
                            $"Sim: Factions: {AvgFactionsMs:F1}ms (1Sec: {AvgFactionOneSecMs:F1}ms, Peak: {PeakFactionOneSecMs:F1}ms) | Cities: {AvgCitiesMs:F1}ms | Map: {AvgMapMs:F1}ms | Input: {AvgUserInputMs:F1}ms | Particles: {AvgParticlesMs:F1}ms{LayoutSeparator}" +
                            $"Eng: Delta: {AvgEngineCalcDeltaMs:F1}ms (Peak: {PeakEngineCalcDeltaMs:F1}ms) | UpdList: {AvgEngineUpdateListMs:F1}ms (Peak: {PeakEngineUpdateListMs:F1}ms) | SyncQue: {AvgEngineSyncQueMs:F1}ms (Peak: {PeakEngineSyncQueMs:F1}ms) | State: {AvgEngineGameStateMs:F1}ms | InSnd: {AvgEngineInputSoundMs:F1}ms | Lazy: {AvgEngineLazyUpdateMs:F1}ms";
        }
    }
}
