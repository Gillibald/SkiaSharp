﻿using System;

namespace SkiaSharp
{
	public unsafe class SKPathEffect : SKObject, ISKReferenceCounted
	{
		[Preserve]
		internal SKPathEffect (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		public static SKPathEffect CreateCompose(SKPathEffect outer, SKPathEffect inner)
		{
			if (outer == null)
				throw new ArgumentNullException(nameof(outer));
			if (inner == null)
				throw new ArgumentNullException(nameof(inner));
			return GetObject (SkiaApi.sk_path_effect_create_compose(outer.Handle, inner.Handle));
		}

		public static SKPathEffect CreateSum(SKPathEffect first, SKPathEffect second)
		{
			if (first == null)
				throw new ArgumentNullException(nameof(first));
			if (second == null)
				throw new ArgumentNullException(nameof(second));
			return GetObject (SkiaApi.sk_path_effect_create_sum(first.Handle, second.Handle));
		}

		public static SKPathEffect CreateDiscrete(float segLength, float deviation, UInt32 seedAssist = 0)
		{
			return GetObject (SkiaApi.sk_path_effect_create_discrete(segLength, deviation, seedAssist));
		}

		public static SKPathEffect CreateCorner(float radius)
		{
			return GetObject (SkiaApi.sk_path_effect_create_corner(radius));
		}

		public static SKPathEffect Create1DPath(SKPath path, float advance, float phase, SKPath1DPathEffectStyle style)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			return GetObject (SkiaApi.sk_path_effect_create_1d_path(path.Handle, advance, phase, style));
		}

		public static SKPathEffect Create2DLine(float width, SKMatrix matrix)
		{
			return GetObject (SkiaApi.sk_path_effect_create_2d_line(width, &matrix));
		}

		public static SKPathEffect Create2DPath(SKMatrix matrix, SKPath path)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			return GetObject (SkiaApi.sk_path_effect_create_2d_path(&matrix, path.Handle));
		}

		public static SKPathEffect CreateDash(float[] intervals, float phase)
		{
			if (intervals == null)
				throw new ArgumentNullException(nameof(intervals));
			if (intervals.Length % 2 != 0)
				throw new ArgumentException("The intervals must have an even number of entries.", nameof(intervals));
			fixed (float* i = intervals) {
				return GetObject (SkiaApi.sk_path_effect_create_dash (i, intervals.Length, phase));
			}
		}

		public static SKPathEffect CreateTrim(float start, float stop)
		{
			return CreateTrim(start, stop, SKTrimPathEffectMode.Normal);
		}

		public static SKPathEffect CreateTrim(float start, float stop, SKTrimPathEffectMode mode)
		{
			return GetObject (SkiaApi.sk_path_effect_create_trim(start, stop, mode));
		}

		internal static SKPathEffect GetObject (IntPtr ptr, bool owns = true, bool unrefExisting = true)
		{
			if (GetInstance<SKPathEffect> (ptr, out var instance)) {
				if (unrefExisting && instance is ISKReferenceCounted refcnt) {
#if THROW_OBJECT_EXCEPTIONS
					if (refcnt.GetReferenceCount () == 1)
						throw new InvalidOperationException (
							$"About to unreference an object that has no references. " +
							$"H: {ptr:x} Type: {instance.GetType ()}");
#endif
					refcnt.SafeUnRef ();
				}
				return instance;
			}

			return new SKPathEffect (ptr, owns);
		}
	}
}

